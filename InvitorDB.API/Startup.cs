using InvitorDB.Data;
using InvitorDB.Models;
using InvitorDB.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InvitorDB.Models.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InvitorDB.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //2. Registraties (van context, Identity) 
            //2.1. Context
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<InvitorDBContext>(options => options.UseSqlServer(connectionString));

            // 2.2 Identity(NIET de AddDefaultIdentity())
            services.AddIdentity<Person, Role>().AddEntityFrameworkStores<InvitorDBContext>();

            services.AddScoped<IPersonRepo, PersonRepo>();

            //4. open API documentatie
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Security_API", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme());
                c.AddSecurityRequirement(new OpenApiSecurityRequirement());
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        AuthorizationCode = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri("https://localhost:5000/connect/authorize"),
                //            TokenUrl = new Uri("https://localhost:5000/connect/token"),
                //            Scopes = new Dictionary<string, string>
                //            {
                //                { "api1", "Demo API - full access" }
                //            }
                //        }
                //    }
                //});
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                    options.SaveToken = false;
                    options.RequireHttpsMetadata = false;
                });

            //5. HSTS & HTTPS-Redirection in production met opties
            if (!env.IsDevelopment())
            {
                services.AddHttpsRedirection(options =>
                {
                    //default: 307 redirect
                    // options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });

                services.AddHsts(options =>
                {
                    options.MaxAge = TimeSpan.FromDays(40); //default 30
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); // vóór mvc oproep !

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //configuratie openAPI documentatie
            app.UseSwagger(); //enable swagger
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger"; //path naar de UI pagina: /swagger/index.html
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Security_API v1.0");
            });
        }
    }
}
