using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Threading;
using Grpc.Core;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CleanArchitecture.Services.Basket.API.Grpc;
using CleanArchitecture.Services.Catalog.API.Grpc;
using CleanArchitecture.Web.BlazorWebAssembly.States;

namespace CleanArchitecture.Web.BlazorWebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services
     .AddBlazorise(options =>
     {
         options.ChangeTextOnKeyPress = true;
     })
     .AddBootstrapProviders()
     .AddFontAwesomeIcons();

            var identityUrl = builder.Configuration.GetSection("IdentityUrl").Value;
            builder.Services.AddHttpClient("CleanArchitecture.Services.Identity.API", client => client.BaseAddress = new Uri(identityUrl))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CleanArchitecture.Services.Identity.API"));

            builder.Services.AddApiAuthorization(a =>
            {
                a.ProviderOptions.ConfigurationEndpoint = $"{identityUrl}/_configuration/CleanArchitecture.Web.BlazorWebAssembly";
            });

           

            builder.Services.AddSingleton(services =>
            {
                // Get the service address from appsettings.json
                var config = services.GetRequiredService<IConfiguration>();
                var catalogUrl = config["CatalogUrl"];

                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWebText, new StreamingHttpHandler(new HttpClientHandler())));
                var channel = GrpcChannel.ForAddress(catalogUrl, new GrpcChannelOptions { HttpClient = httpClient });

                return new Product.ProductClient(channel);
            });
            builder.Services.AddSingleton(services =>
            {
                // Get the service address from appsettings.json
                var config = services.GetRequiredService<IConfiguration>();
                var basketUrl = config["BasketUrl"];

                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWebText, new StreamingHttpHandler(new HttpClientHandler())));
                var channel = GrpcChannel.ForAddress(basketUrl, new GrpcChannelOptions { HttpClient = httpClient });

                return new Basket.BasketClient(channel);
            });
            builder.Services.AddSingleton<BasketState>();
            var host = builder.Build();
            host.Services
      .UseBootstrapProviders()
      .UseFontAwesomeIcons();
            await host.RunAsync();
        }
        private class StreamingHttpHandler : DelegatingHandler
        {
            public StreamingHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
            {
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.SetBrowserResponseStreamingEnabled(true);
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
