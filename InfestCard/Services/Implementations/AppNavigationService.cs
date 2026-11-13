using InfestCard.Models;
using InfestCard.Services.Interfaces;
using InfestCard.Views;

public sealed class AppNavigationService(IServiceProvider serviceProvider)
    : IAppNavigationService
{
    private INavigation Navigation =>
        Application.Current!.Windows[0].Page!.Navigation;

    private Task PushAsync(Page page)
    {
        NavigationPage.SetHasNavigationBar(page, false);
        return Navigation.PushAsync(page, true);
    }

    public Task NavigateToScannerAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var page = serviceProvider.GetRequiredService<ScannerPage>();

        return PushAsync(page);
    }

    public Task CloseScannerAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Navigation.PopAsync(true);
    }

    public Task NavigateToCardDetailAsync(
        Card card,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        cancellationToken.ThrowIfCancellationRequested();

        var page = serviceProvider.GetRequiredService<CardDetailPage>();
        page.ViewModel.Load(card);

        return PushAsync(page);
    }

    public Task NavigateBackToCollectionAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Navigation.PopAsync(true);
    }

    public Task NavigateToQrAsync(
        Card card,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        cancellationToken.ThrowIfCancellationRequested();

        var page = serviceProvider.GetRequiredService<QrCodePage>();
        page.Load(card);

        return PushAsync(page);
    }
}
