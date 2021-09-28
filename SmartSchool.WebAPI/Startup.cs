using SmartSchool.WebAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;

namespace SmartSchool.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            /* ADDSingleton
            Cria uma únioca instância do serviço quando é solicitado pela
            primeira vez e reutiliza essa mesma instância em todos os locais
            em que esse serviço é necessário.
            ****Problema
            Ele compartilha a memoria.            

            services.AddSingleton<IRepository, Repository>();
            */

            /* AddTransient
            Sempre gerará um nova instância para cada item encontrado que possua
            tal dependência, ou seja, se houver 5 dependências serão 5 instâncias diferentes
            
            
            services.AddTransient<IRepository, Repository>();
            */


            /*AddScope
            Essa é diferente da Transient que garante que uma requisição
            seja criada um instância de uma classe onde se houver outras
            dependências, seja utilizada essa única instância pra todas,
            renovando somente nas requisições subsequentes, mas mantendo
            essa obrigatoriedade
            */

            //Conexao Via SQLite
            services.AddDbContext<SmartContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            services.AddControllers().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepository, Repository>();

            //Versionamento
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVVV";
                options.SubstituteApiVersionInUrl = true;

            }).AddApiVersioning(options =>
            {

                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;

            });


            var apiProviderDescription = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();


            services.AddSwaggerGen(options =>
            {

                foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                {

                    //URL: editor.swagger.io
                    //Criar documentação da API Swagger 
                    options.SwaggerDoc(
                        description.GroupName,
                         new OpenApiInfo()
                         {
                             Title = "SmartSchool API",
                             Version = description.ApiVersion.ToString(),
                             TermsOfService = new Uri("https://SeusTermosDeUso.com"),
                             Description = "A Descrição da WebApi do SmartSchool",
                             License = new Microsoft.OpenApi.Models.OpenApiLicense
                             {
                                 Name = "SmartSchool License",
                                 Url = new Uri("http://mit.com")
                             },
                             Contact = new Microsoft.OpenApi.Models.OpenApiContact
                             {
                                 Name = "Victor Rodrigues",
                                 Email = "",
                                 Url = new Uri("http://programadamente.com")
                             }
                         });

                }


                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescription)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSchool.WebAPI v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescription.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        options.RoutePrefix = "";
                    }
                });

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
