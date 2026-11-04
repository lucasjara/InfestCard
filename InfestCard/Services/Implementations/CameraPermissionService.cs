using Microsoft.Maui.ApplicationModel;
using InfestCard.Services.Interfaces;

namespace InfestCard.Services.Implementations;

public sealed class CameraPermissionService : ICameraPermissionService
{
    public async Task<PermissionStatus> RequestPermissionAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Denied && status != PermissionStatus.Unknown)
        {
            return status;
        }

        return await Permissions.RequestAsync<Permissions.Camera>();
    }
}
