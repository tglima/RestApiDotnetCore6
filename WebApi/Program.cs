using System.Net;
using WebApi.Helpers;
using WebApi.Services;
using System.Reflection;
using WebApi.Middlewares;
using System.ComponentModel;
using WebApi.Models.Examples;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace WebApi
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var nuVersion = AppHelper.GetNuVersion();
            var nmApplication = AppHelper.GetNmApplication();
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddSwaggerExamplesFromAssemblyOf<InvalidRequestExample>();
            builder.Services.AddSwaggerExamplesFromAssemblyOf<AuthFailedExample>();
            builder.Services.AddSwaggerExamplesFromAssemblyOf<NotFoundExample>();
            builder.Services.AddSwaggerExamplesFromAssemblyOf<ServerErrorExample>();
            builder.Services.AddSwaggerExamplesFromAssemblyOf<ProductExample>();
            builder.Services.AddSwaggerExamplesFromAssemblyOf<ArrayProductExample>();

            #region Configura o Swagger
            builder.Services.AddSwaggerGen(o =>
            {
                o.ExampleFilters();
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

                o.AddSecurityDefinition(Constant.API_KEY_HEADER, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = Constant.SwaggerSecurityDescription,
                    Name = Constant.API_KEY_HEADER,
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

            #region Carrega tabelas do SQLite
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DbSQLiteContext>();
                context.Init().Wait();
            }
            #endregion


            #region Carrega o Swagger

            //Adicionado para permitir carregar o css personalizado
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"{nmApplication}{Constant.JsonSwaggerFile}", Constant.SwaggerTitlePage);
                s.RoutePrefix = "swagger";
                s.DocumentTitle = Constant.SwaggerTitlePage;
                s.DocExpansion(DocExpansion.None);
                s.InjectStylesheet(Constant.CssSwaggerFilePath);
            });

            #endregion

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.UseApiHandlerMiddleware();


            app.Run();

        }
    }
}




