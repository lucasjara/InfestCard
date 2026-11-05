using InfestCard.ViewModels;
using ZXing.Net.Maui;

namespace InfestCard.Views;

public partial class ScannerPage : ContentPage
{
    private ScannerViewModel ViewModel => (ScannerViewModel)BindingContext;

    public ScannerPage(ScannerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        BarcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false,
            TryHarder = true,
            TryInverted = true
        };
    }

    private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs eventArgs)
    {
        var resultCount = eventArgs.Results.Count();
        System.Diagnostics.Debug.WriteLine($"[INFESTCard][QR] Resultados detectados: {resultCount}");

        var value = eventArgs.Results.FirstOrDefault()?.Value;
        if (string.IsNullOrWhiteSpace(value))
        {
            System.Diagnostics.Debug.WriteLine("[INFESTCard][QR] El resultado no contiene texto.");
            return;
        }

        System.Diagnostics.Debug.WriteLine(
            $"[INFESTCard][QR] Contenido detectado. Longitud: {value.Length}. " +
            $"Inicio: {value[..Math.Min(value.Length, 120)]}");

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await ViewModel.ProcessCodeCommand.ExecuteAsync(value);
        });
    }
    
}
