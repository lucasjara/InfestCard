using InfestCard.ViewModels;
using Microsoft.Maui.Controls.Shapes;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using System.Linq;

namespace InfestCard.Views;

public partial class ScannerPage : ContentPage
{
    private ScannerViewModel ViewModel =>
        (ScannerViewModel)BindingContext;

    private CameraBarcodeReaderView BarcodeReader = null!;

    private Border? ScannerFrame;
    private BoxView? ScanLine;

    private CancellationTokenSource? animationCancellation;
    private bool hasPlayedEntrance;

    public ScannerPage(ScannerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ViewModel.OnPageAppearingAsync();

        if (!hasPlayedEntrance)
        {
            hasPlayedEntrance = true;
            await PlayEntranceAnimationAsync();
        }

        await RestartScannerAsync();
    }

    protected override void OnDisappearing()
    {
        StopScannerAnimations();

        if (BarcodeReader is not null)
        {
            BarcodeReader.IsDetecting = false;
            BarcodeReader.CancelAnimations();
        }

        base.OnDisappearing();
    }

    // Entrada de la pantalla.
    private async Task PlayEntranceAnimationAsync()
    {
        await Task.WhenAll(
            HeaderView.FadeToAsync(1, 250, Easing.CubicOut),
            HeaderView.TranslateToAsync(0, 0, 250, Easing.CubicOut),
            ControlsView.FadeToAsync(1, 300, Easing.CubicOut),
            ControlsView.TranslateToAsync(0, 0, 300, Easing.CubicOut)
        );
    }

    // Recrear el lector para inicializar correctamente la cámara.
    private async Task RestartScannerAsync()
    {
        StopScannerAnimations();

        ScannerContainer.Children.Clear();

        var scanner = new CameraBarcodeReaderView
        {
            Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormat.QrCode,
                AutoRotate = true,
                Multiple = false,
                TryHarder = true,
                TryInverted = true
            },
            IsDetecting = false,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        scanner.BarcodesDetected += OnBarcodesDetected;

        // Línea de escaneo.
        ScanLine = new BoxView
        {
            HeightRequest = 3,
            WidthRequest = 240,
            Color = Color.FromArgb("#E879F9"),
            Opacity = 0,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            InputTransparent = true
        };

        // Marco del escáner.
        ScannerFrame = new Border
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
            InputTransparent = true,
            Opacity = 0
        };

        // Orden de capas: cámara, línea y marco.
        ScannerContainer.Children.Add(scanner);
        ScannerContainer.Children.Add(ScanLine);
        ScannerContainer.Children.Add(ScannerFrame);

        BarcodeReader = scanner;
        BarcodeReader.ZoomFactor = (float)ZoomSlider.Value;

        await Task.Delay(500);

        if (!IsVisible || !ViewModel.IsDetecting)
            return;

        BarcodeReader.IsDetecting = true;

        StartScannerAnimations();
    }

    // Iniciar animaciones mientras la cámara está activa.
    private void StartScannerAnimations()
    {
        if (ScannerFrame is null || ScanLine is null)
            return;

        StopScannerAnimations();

        animationCancellation = new CancellationTokenSource();

        ScannerFrame.Opacity = 1;
        ScannerFrame.Scale = 1;

        ScanLine.Opacity = 0.9;
        ScanLine.TranslationY = -105;

        var token = animationCancellation.Token;

        _ = RunScannerAnimationsAsync(token);
    }

    private async Task RunScannerAnimationsAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            if (ScannerFrame is null || ScanLine is null)
                return;

            var frameAnimation = AnimateFrameAsync(cancellationToken);
            var lineAnimation = AnimateScanLineAsync(cancellationToken);

            await Task.WhenAll(frameAnimation, lineAnimation);
        }
        catch (OperationCanceledException)
        {
            // Las animaciones se detuvieron intencionalmente.
        }
    }

    // Pulso suave del marco.
    private async Task AnimateFrameAsync(
        CancellationToken cancellationToken)
    {
        if (ScannerFrame is null)
            return;

        while (!cancellationToken.IsCancellationRequested)
        {
            await ScannerFrame.ScaleToAsync(
                1.025,
                700,
                Easing.SinInOut);

            if (cancellationToken.IsCancellationRequested)
                break;

            await ScannerFrame.ScaleToAsync(
                1,
                700,
                Easing.SinInOut);
        }
    }

    // Movimiento vertical de la línea de escaneo.
    private async Task AnimateScanLineAsync(
        CancellationToken cancellationToken)
    {
        if (ScanLine is null)
            return;

        while (!cancellationToken.IsCancellationRequested)
        {
            await ScanLine.TranslateToAsync(
                0,
                105,
                1300,
                Easing.SinInOut);

            if (cancellationToken.IsCancellationRequested)
                break;

            await ScanLine.TranslateToAsync(
                0,
                -105,
                1300,
                Easing.SinInOut);
        }
    }

    // Detener todas las animaciones.
    private void StopScannerAnimations()
    {
        animationCancellation?.Cancel();
        animationCancellation?.Dispose();
        animationCancellation = null;

        ScannerFrame?.CancelAnimations();
        ScanLine?.CancelAnimations();

        if (ScannerFrame is not null)
            ScannerFrame.Scale = 1;

        if (ScanLine is not null)
            ScanLine.Opacity = 0;
    }

    // Lectura de códigos QR.
    private async void OnBarcodesDetected(
        object? sender,
        BarcodeDetectionEventArgs eventArgs)
    {
        var value = eventArgs.Results.FirstOrDefault()?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (ViewModel.IsBusy || !ViewModel.IsDetecting)
                return;

            BarcodeReader.IsDetecting = false;

            StopScannerAnimations();

            System.Diagnostics.Debug.WriteLine(
                $"[INFESTCard][QR] Contenido detectado. Longitud: {value.Length}");

            await ViewModel.ProcessCodeCommand.ExecuteAsync(value);
        });
    }

    // Control del zoom.
    private void OnZoomChanged(
        object sender,
        ValueChangedEventArgs e)
    {
        if (BarcodeReader is null)
            return;

        BarcodeReader.ZoomFactor = (float)e.NewValue;
    }

    // Seleccionar QR desde galería.
    private async void OnSelectImageClicked(
        object sender,
        EventArgs e)
    {
        if (ViewModel.IsBusy)
            return;

        BarcodeReader.IsDetecting = false;
        StopScannerAnimations();

        try
        {
            var photos = await MediaPicker.Default.PickPhotosAsync();

            var photo = photos?.FirstOrDefault();

            if (photo is null)
                return;

            using var stream = await photo.OpenReadAsync();

            var results = await ZXing.Net.Maui.BarcodeReader.DecodeAsync(
                stream,
                new BarcodeReaderOptions
                {
                    Formats = BarcodeFormat.QrCode,
                    AutoRotate = true,
                    TryHarder = true,
                    TryInverted = true,
                    Multiple = false
                });

            var qr = results?.FirstOrDefault();

            if (qr is null || string.IsNullOrWhiteSpace(qr.Value))
            {
                await DisplayAlertAsync(
                    "QR no encontrado",
                    "La imagen seleccionada no contiene un código QR válido.",
                    "Aceptar");

                return;
            }

            System.Diagnostics.Debug.WriteLine(
                "[INFESTCard][QR] Código detectado desde galería.");

            await ViewModel.ProcessCodeCommand.ExecuteAsync(qr.Value);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[INFESTCard][QR] Error al procesar imagen: {ex}");

            await DisplayAlertAsync(
                "Error",
                "No fue posible procesar la imagen seleccionada.",
                "Aceptar");
        }
        finally
        {
            if (IsVisible &&
                ViewModel.IsDetecting &&
                !ViewModel.IsBusy)
            {
                BarcodeReader.IsDetecting = true;
                StartScannerAnimations();
            }
        }
    }
}