using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using PamiwPwa;
using PamiwPwa.Security;
using PamiwShared.Services;
using Microsoft.JSInterop;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
builder.Services.AddSingleton<IAuthorService, AuthorService>();
builder.Services.AddSingleton<IPublisherService, PublisherService>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddSingleton<AuthState>();
builder.Services.AddSingleton<ThemeSettings>();

builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddLocalization();

var host = builder.Build();

CultureInfo culture;
var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");

if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo("en-US");
    await js.InvokeVoidAsync("blazorCulture.set", "en-US");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var themeSettings = host.Services.GetRequiredService<ThemeSettings>();
var storageTheme = await js.InvokeAsync<string>("appTheme.get");
AppTheme initialTheme;

if (storageTheme != null)
{
    initialTheme = storageTheme switch
    {
        "Dark" => AppTheme.Dark,
        "Light" or _ => AppTheme.Light
    };
} else
{
    initialTheme = AppTheme.Light;
    await js.InvokeVoidAsync("appTheme.set", "Light");
}

themeSettings.SetTheme(initialTheme);

await host.RunAsync();
