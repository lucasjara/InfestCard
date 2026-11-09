using InfestCard.Models;
using InfestCard.Services.Interfaces;
using InfestCard.Views;

public sealed class AppNavigationService(IServiceProvider serviceProvider) : IAppNavigationService
{
    public Task NavigateToScannerAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var scannerPage = serviceProvider.GetRequiredService<ScannerPage>();
        return Shell.Current.Navigation.PushAsync(scannerPage);
    }

    public Task CloseScannerAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Shell.Current.Navigation.PopAsync();
    }

    public Task NavigateToCardDetailAsync(Card card, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        cancellationToken.ThrowIfCancellationRequested();

        var detailPage = serviceProvider.GetRequiredService<CardDetailPage>();
        detailPage.ViewModel.Load(card);
        return Shell.Current.Navigation.PushAsync(detailPage);
    }

    public Task NavigateBackToCollectionAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Shell.Current.Navigation.PopAsync();
    }
    public Task NavigateToQrAsync(Card card,CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        cancellationToken.ThrowIfCancellationRequested();

        var qrPage = serviceProvider.GetRequiredService<QrCodePage>();
        qrPage.Load(card);

        return Shell.Current.Navigation.PushAsync(qrPage);
    }
}