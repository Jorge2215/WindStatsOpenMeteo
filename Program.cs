using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WindStatsOpenMeteo.Components;
using WindStatsOpenMeteo.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Named HTTP clients for external APIs
builder.Services.AddHttpClient("OpenMeteo", client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddHttpClient("OpenMeteoGeo", client =>
{
    client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/v1/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpClient("Nominatim", client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.Add("User-Agent", "WindStatsOpenMeteo/1.0 (educational-blazor-app)");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Application services
builder.Services.AddScoped<OpenMeteoService>();
builder.Services.AddScoped<GeocodingService>();

await builder.Build().RunAsync();
