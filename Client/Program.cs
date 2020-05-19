using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CarChecker.Client.Data;

namespace CarChecker.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            // Configure HttpClient for use when talking to server backend
            builder.Services.AddHttpClient("CarChecker.ServerAPI",
                client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Other DI services
            builder.Services.AddScoped<LocalVehiclesStore>();
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CarChecker.ServerAPI"));
            builder.Services.AddApiAuthorization();
            builder.Services.AddScoped<AccountClaimsPrincipalFactory<RemoteUserAccount>, OfflineAccountClaimsPrincipalFactory>();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            await builder.Build().RunAsync();
        }
    }
}
