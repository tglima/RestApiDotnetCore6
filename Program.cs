using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Services.AddControllers();
builder.Services.AddSingleton<DbSQLiteContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Configura o Swagger
builder.Services.AddSwaggerGen(o =>
{
    string nuVersion = "0.1";
    string nmApplication = "WebApi";

    o.EnableAnnotations();
    o.SwaggerDoc(nuVersion, new OpenApiInfo { Title = nmApplication, Version = nuVersion });
    o.AddSecurityDefinition("API-KEY", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Chave de acesso individual disponibilizado para acessar a API",
        Name = "API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    o.CustomSchemaIds(x => x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? x.Name);

    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    o.IncludeXmlComments(xmlPath);


});

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
#region Carrega o Swagger
app.UseSwagger();
app.UseSwaggerUI(s =>
{

    var nuVersion = "0.1";
    var nmApplication = "WebApi";

    s.SwaggerEndpoint($"{nuVersion}/swagger.json", $"{nmApplication} {nuVersion}");
    s.DocumentTitle = "tglimatech - WebApi";
    s.DocExpansion(DocExpansion.None);
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
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
