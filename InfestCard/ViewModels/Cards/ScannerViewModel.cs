using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using InfestCard.Services.Interfaces;
using Microsoft.Maui.ApplicationModel;

namespace InfestCard.ViewModels;

public partial class ScannerViewModel : BaseViewModel
{
    private readonly ICameraPermissionService cameraPermissionService;
    private readonly IQrScannerService qrScannerService;
    private readonly ICardApiService cardApiService;
    private readonly ICardImageService cardImageService;
    private readonly ICardStorageService cardStorageService;
    private readonly IAppNavigationService appNavigationService;

    public async Task OnPageAppearingAsync()
    {
        await InitializeAsync();
    }
    public ScannerViewModel(
        ICameraPermissionService cameraPermissionService,
        IQrScannerService qrScannerService,
        ICardApiService cardApiService,
        ICardImageService cardImageService,
        ICardStorageService cardStorageService,
        IAppNavigationService appNavigationService)
    {
        this.cameraPermissionService = cameraPermissionService;
        this.qrScannerService = qrScannerService;
        this.cardApiService = cardApiService;
        this.cardImageService = cardImageService;
        this.cardStorageService = cardStorageService;
        this.appNavigationService = appNavigationService;
    }

    public bool IsDetecting { get; private set; }

    public string? DetectedCode { get; private set; }

    public string? DetectedCardName { get; private set; }

    public string StatusMessage { get; private set; } = "Solicitando permiso de cámara...";

    [RelayCommand]
    private async Task ProcessCodeAsync(string value)
    {
        var acceptedCode = qrScannerService.AcceptCode(value);
        if (acceptedCode is null)
        {
            return;
        }

        IsDetecting = false;
        OnPropertyChanged(nameof(IsDetecting));

        IsBusy = true;
        ErrorMessage = null;
        StatusMessage = $"QR detectado ({acceptedCode.Length} caracteres). Procesando...";
        OnPropertyChanged(nameof(StatusMessage));

        System.Diagnostics.Debug.WriteLine(
            $"[INFESTCard][QR] Procesando payload. Longitud: {acceptedCode.Length}. " +
            $"Inicio: {acceptedCode[..Math.Min(acceptedCode.Length, 120)]}");

        try
        {
            var card = await cardApiService.GetCardFromQrAsync(acceptedCode);
            card = await cardImageService.PrepareImageAsync(card);
            await cardStorageService.SaveCardAsync(card);

            DetectedCode = acceptedCode;
            DetectedCardName = card.Name;
            StatusMessage = "Carta guardada en tu colección.";
            OnPropertyChanged(nameof(DetectedCode));
            OnPropertyChanged(nameof(DetectedCardName));
            OnPropertyChanged(nameof(StatusMessage));

            await appNavigationService.CloseScannerAsync();
        }
        catch (Exception exception) when (exception is InvalidDataException or JsonException)
        {
            ErrorMessage = exception.Message;
            StatusMessage = "No se pudo procesar la carta.";
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(ErrorMessage));
            System.Diagnostics.Debug.WriteLine(exception);
        }
        catch (Exception exception)
        {
            ErrorMessage = $"No se pudo guardar la carta: {exception.Message}";
            StatusMessage = "Ocurrió un error al guardar la carta.";
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(ErrorMessage));
            System.Diagnostics.Debug.WriteLine(exception);
        }
        finally
        {
            IsBusy = false;
        }
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
