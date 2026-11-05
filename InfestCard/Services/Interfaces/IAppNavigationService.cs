using InfestCard.Models;

namespace InfestCard.Services.Interfaces;

public interface IAppNavigationService
{
    Task NavigateToScannerAsync(CancellationToken cancellationToken = default);

    Task NavigateToCardDetailAsync(Card card, CancellationToken cancellationToken = default);
}
