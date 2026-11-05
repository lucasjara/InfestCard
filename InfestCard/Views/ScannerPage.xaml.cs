using InfestCard.ViewModels;
#if ANDROID
using Java.Util;
#endif
using Microsoft.Maui.Controls.Shapes;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

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
    private void OnZoomChanged(object sender, ValueChangedEventArgs e)
    {
        BarcodeReader.ZoomFactor = (float)e.NewValue;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ViewModel.OnPageAppearingAsync();

        RestartScanner();
    }

    protected override void OnDisappearing()
    {
        BarcodeReader.IsDetecting = false;

        base.OnDisappearing();
    }
    private void RestartScanner()
    {
        ScannerContainer.Children.Clear();

        var scanner = new CameraBarcodeReaderView
        {
            Options = BarcodeReader.Options,
            IsDetecting = false
        };

        scanner.BarcodesDetected += OnBarcodesDetected;
        var frame = new Border
        {
            Margin = new Thickness(8),
            Stroke = Color.FromArgb("#C026D3"),
            StrokeThickness = 2,
            BackgroundColor = Colors.Transparent,
            WidthRequest = 304,
            HeightRequest = 304,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(12)
            },
            InputTransparent = true
        };

        ScannerContainer.Children.Add(scanner);
        ScannerContainer.Children.Add(frame);

        BarcodeReader = scanner;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(500);
            scanner.IsDetecting = ViewModel.IsDetecting;
        });
    }
}
