using InfestCard.Services.Interfaces;
using InfestCard.Views;
using Microsoft.Extensions.DependencyInjection;

namespace InfestCard.Services.Implementations;

public sealed class AppNavigationService(IServiceProvider serviceProvider) : IAppNavigationService
{
    public Task NavigateToScannerAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var scannerPage = serviceProvider.GetRequiredService<ScannerPage>();
        return Shell.Current.Navigation.PushAsync(scannerPage);
    }
}
