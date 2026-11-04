namespace InfestCard.Services.Interfaces;

public interface IAppNavigationService
{
    Task NavigateToScannerAsync(CancellationToken cancellationToken = default);
}
