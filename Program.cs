using System.Net;
using WebApi.Helpers;
using WebApi.Services;
using System.Reflection;
using WebApi.Middlewares;
using System.ComponentModel;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var nuVersion = AppHelper.GetNuVersion();
var nmApplication = AppHelper.GetNmApplication();
var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

#region Força utilizacao do TLS 1.2
ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
ServicePointManager.ServerCertificateValidationCallback +=
(se, cert, chain, sslerror) =>
{
    return true;
};
#endregion


builder.Services.AddControllers();
builder.Services.AddTransient<ProductService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddSingleton<DbSQLiteContext>();
builder.Services.AddEndpointsApiExplorer();

#region Configura o Swagger
builder.Services.AddSwaggerGen(o =>
{

    o.EnableAnnotations();
    o.SwaggerDoc(nmApplication, new OpenApiInfo
    {
        Title = Constant.SwaggerTitle,
        Description = Constant.SwaggerDescription,
        Version = nuVersion,
        Contact = new OpenApiContact
        {
            Name = Constant.SwaggerContactName,
            Url = new Uri(Constant.SwaggerContactUrl)
        },
        License = new OpenApiLicense
        {
            Name = Constant.SwaggerLicenseName,
            Url = new Uri(Constant.SwaggerLicenseUrl)
        }

    });

    o.AddSecurityDefinition(Constant.API_KEY, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = Constant.SwaggerSecurityDescription,
        Name = Constant.API_KEY,
        Type = SecuritySchemeType.ApiKey,
        Scheme = Constant.SwaggerSecurityScheme
    });

    o.OperationFilter<SwaggerAllowAnonymousOperationFilter>();

    o.CustomSchemaIds(x => x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? x.Name);

    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    o.IncludeXmlComments(xmlPath);

});

#endregion


var app = builder.Build();

app.UseApiHandlerMiddleware();
// Configure the HTTP request pipeline.
#region Carrega o Swagger
app.UseSwagger();

app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();

app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint(Constant.JsonSwaggerFile, Constant.SwaggerTitlePage);
    s.RoutePrefix = "swagger";
    s.DocumentTitle = Constant.SwaggerTitlePage;
    s.DocExpansion(DocExpansion.None);
    s.InjectStylesheet(Constant.CssSwaggerFilePath);
});

app.UseEndpoints(endpoints =>
     {
         endpoints.MapGet(Constant.JsonSwaggerFile, context =>
        {
            context.Response.Redirect($"/swagger/{nmApplication}{Constant.JsonSwaggerFile}");
            return Task.CompletedTask;
        });
     });
#endregion

// Garante que o Database Sqlite e suas tabelas serão criados
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DbSQLiteContext>();
    await context.Init();
}

// Redireciona todos as requisições / para /swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" || context.Request.Path == "/index.html")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseHttpsRedirection();

app.MapControllers();



app.Run();
