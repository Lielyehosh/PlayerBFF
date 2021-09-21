using System;
using System.Text;
using AuthService.Models;
using BFF.Service.Interfaces;
using Common.Models;
using Common.Utils;
using Common.Utils.Extensions;
using Common.Utils.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BFF.Service
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
            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddMongoDal<GameDatabase>();
            services.AddSingleton<IAuthService, Services.AuthService>();
            // Add GRPC Ms Client
            services.AddAuthMsClient(new GrpcClientSettings()
            {
                Address = Configuration["AuthMsGrpc:Address"],
                IgnoreSsl = Convert.ToBoolean(Configuration["AuthMsGrpc:IgnoreSsl"])
            });
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMongoDb(Configuration["MongoDBConnectionString"]);
            app.UseHttpsRedirection();

            app.UseRouting();
            // TODO - figure out how it should be deployed 
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            
            app.UseAuthentication();
            app.UseAuthorization();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(Convert.ToDouble(Configuration["WebSocket:KeepAliveInterval"])),
            };
            app.UseWebSockets(webSocketOptions);
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}