using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GRUWebAssembly;
using GRUComponents.Services;
using GRULib;
using System.Net.Http;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var response = await http.GetAsync("quickbooksettings_prod.json");
response.EnsureSuccessStatusCode();
var json = await response.Content.ReadAsStringAsync();
var quickBooksSettings = JsonSerializer.Deserialize<QuickBooksSettings>(json)
    ?? throw new Exception("Failed to deserialize QuickBooksSettings");

builder.Services.AddSingleton(quickBooksSettings);
builder.Services.AddSingleton<QuickBooksService>();
builder.Services.AddSingleton<LoginEventService>();

await builder.Build().RunAsync();
