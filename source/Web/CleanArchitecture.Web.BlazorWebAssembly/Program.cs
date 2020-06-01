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
                var backendUrl = config["CatalogUrl"];

                // Create a gRPC-Web channel pointing to the backend server.
                //
                // GrpcWebText is used because server streaming requires it. If server streaming is not used in your app
                // then GrpcWeb is recommended because it produces smaller messages.
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWebText, new StreamingHttpHandler(new HttpClientHandler())));

                var channel = GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpClient = httpClient });

                return channel;
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
