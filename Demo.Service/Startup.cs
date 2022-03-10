using Demo.Service._Business.Consumers;
using Demo.Service.Middlewares;
using Demo.Service.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace Demo.Service
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextRegister(Configuration);
            services.AddJwtRegister(Configuration);
            services.AddManagerRegister(Configuration);
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo.Service", Version = "v1" });
                // Define the BearerAuth scheme that's in use
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                     }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: _defaultCorsPolicyName,
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("CorsOrigins").Value.Split(",", StringSplitOptions.RemoveEmptyEntries)).AllowAnyHeader()
                                                  .AllowAnyMethod();
                    });
            });

            // config for RabbitMQ   
            services.AddMassTransit(x => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                x.AddConsumer<UpdateConsumer>();                
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri(Environment.GetEnvironmentVariable("RABBIT_MQ")), host =>                    
                    {
                        host.Username("user");
                        host.Password("bitnami");
                    });
                    config.ReceiveEndpoint("UpdateTitle", configuaration =>
                    {
                        configuaration.ConfigureConsumer<UpdateConsumer>(context);
                    });                    
                });                

                x.SetKebabCaseEndpointNameFormatter();

            }));

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.Service v1"));
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
