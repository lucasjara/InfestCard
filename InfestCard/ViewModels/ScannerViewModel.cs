using CommunityToolkit.Mvvm.Input;
using InfestCard.Services.Interfaces;
using Microsoft.Maui.ApplicationModel;

namespace InfestCard.ViewModels;

public partial class ScannerViewModel : BaseViewModel
{
    private readonly ICameraPermissionService cameraPermissionService;
    private readonly IQrScannerService qrScannerService;

    public ScannerViewModel(
        ICameraPermissionService cameraPermissionService,
        IQrScannerService qrScannerService)
    {
        this.cameraPermissionService = cameraPermissionService;
        this.qrScannerService = qrScannerService;
        _ = InitializeAsync();
    }

    public bool IsDetecting { get; private set; }

    public string? DetectedCode { get; private set; }

    public string StatusMessage { get; private set; } = "Solicitando permiso de cámara...";

    [RelayCommand]
    private async Task ProcessCodeAsync(string value)
    {
        var acceptedCode = qrScannerService.AcceptCode(value);
        if (acceptedCode is null)
        {
            return;
        }

        DetectedCode = acceptedCode;
        IsDetecting = false;
        StatusMessage = "Código QR leído. La consulta de la carta se implementará en la siguiente etapa.";
        OnPropertyChanged(nameof(DetectedCode));
        OnPropertyChanged(nameof(IsDetecting));
        OnPropertyChanged(nameof(StatusMessage));
        await Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var permissionStatus = await cameraPermissionService.RequestPermissionAsync();
            IsDetecting = permissionStatus == PermissionStatus.Granted;
            StatusMessage = IsDetecting
                ? "Apunta la cámara a un código QR de INFESTCard."
                : "El permiso de cámara es necesario para escanear códigos QR.";

            OnPropertyChanged(nameof(IsDetecting));
            OnPropertyChanged(nameof(StatusMessage));
        }
        catch (Exception exception)
        {
            IsDetecting = false;
            ErrorMessage = "No se pudo obtener el permiso de cámara.";
            StatusMessage = "No se puede iniciar el escáner.";
            OnPropertyChanged(nameof(IsDetecting));
            OnPropertyChanged(nameof(StatusMessage));
            System.Diagnostics.Debug.WriteLine(exception);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
