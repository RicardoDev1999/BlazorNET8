using BlazorApp.Services.Concrete;

namespace BlazorApp.BackgroundServices
{
    public class WildPokemonBackgroundService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;

        private Timer? _timer = null;

        public WildPokemonBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _services.CreateScope())
            {
                var wildPokemonService =
                    scope.ServiceProvider
                        .GetRequiredService<WildPokemonService>();

                _ = Task.Run(wildPokemonService.GenerateWildPokemon);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
