using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PamiwPwa;
using PamiwPwa.Security;
using PamiwShared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
builder.Services.AddSingleton<IAuthorService, AuthorService>();
builder.Services.AddSingleton<IPublisherService, PublisherService>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddSingleton<AuthState>();

builder.Services.AddLocalization();

await builder.Build().RunAsync();
