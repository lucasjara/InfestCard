using Microsoft.Maui.ApplicationModel;

namespace InfestCard.Services.Interfaces;

public interface ICameraPermissionService
{
    Task<PermissionStatus> RequestPermissionAsync(CancellationToken cancellationToken = default);
}
