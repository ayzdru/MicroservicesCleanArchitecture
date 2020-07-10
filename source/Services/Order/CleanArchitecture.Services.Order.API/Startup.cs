using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Services.Order.API.Data;
using CleanArchitecture.Services.Order.API.Grpc;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using DotNetCore.CAP.Messages;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Order.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("OrderConnectionString")));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "order";
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddAuthorization();
            services.AddGrpc();
            services.AddGrpcWeb(o => o.GrpcWebEnabled = true);
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            services.AddCap(x =>
            {
                x.UseEntityFramework<OrderDbContext>();
                x.UseRabbitMQ("localhost");
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = failed =>
                {
                    var aaa =
                        $@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}";
                };
            });

            services.AddHttpContextAccessor();
            var basketUrl = Configuration.GetValue<string>("BasketUrl");
            var channel = GrpcChannel.ForAddress(basketUrl);
            var client = new CleanArchitecture.Services.Basket.API.Grpc.Basket.BasketClient(channel);
            services.AddSingleton(client);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseGrpcWeb();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrderService>().RequireCors("CorsPolicy");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Order MicroService");
                });
            });
        }
    }
}
