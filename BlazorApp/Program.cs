using BlazorApp.BackgroundServices;
using BlazorApp.Clients;
using BlazorApp.Clients.Default;
using BlazorApp.Components;
using BlazorApp.Services.Concrete;
using BlazorApp.Services.Interfaces;
using BlazorApp.Stores;
using Serilog;

namespace BlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Host.UseSerilog((ctx, loggerConfig) =>
                loggerConfig.ReadFrom.Configuration(ctx.Configuration));

#if DEBUG
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "Redis_";
            });
#else
            builder.Services.AddDistributedMemoryCache();
#endif

            builder.Services.AddHttpClient<IPokemonApiClient, ApiClient>(opt =>
            {
                opt.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
            });

            //Add concrete services
            builder.Services
                .AddTransient<IPokemonService, PokemonService>()
                .AddTransient<IFavoritePokemonService, FavoritePokemonService>()
                .AddSingleton<WildPokemonService>();

            //Add stores
            builder.Services
                .AddScoped<PokemonListStore>()
                .AddScoped<PokemonDetailStore>();

            builder.Services
                .AddHostedService<WildPokemonBackgroundService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
