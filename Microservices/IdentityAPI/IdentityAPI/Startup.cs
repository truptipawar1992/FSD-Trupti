using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityAPI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace IdentityAPI
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
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<IdentityDbContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("IdentitySqlConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Identity API",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Email = "virals@hexaware.com",
                        Name = "Viral Shah"
                    }
                });
            });

            services.AddCors(c =>
            {
                //c.AddPolicy("All", builder =>
                //{
                //    builder.AllowAnyOrigin()
                //            .WithMethods("POST")
                //            .AllowAnyHeader();
                //});
                //c.DefaultPolicyName = "All";

                //This is default policy which you can specify instead of above two line
                c.AddDefaultPolicy(builder => {
                    builder.AllowAnyOrigin()
                           .WithMethods("POST")
                           .AllowAnyHeader();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity");
                    c.RoutePrefix = "";
                });
            }

            app.UseCors();

            app.UseMvc();
        }
    }
}
