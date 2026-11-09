
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

    private async void OnBarcodesDetected(
        object? sender,
        BarcodeDetectionEventArgs eventArgs)
    {
        var value = eventArgs.Results.FirstOrDefault()?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return;

        System.Diagnostics.Debug.WriteLine(
            $"[INFESTCard][QR] Contenido detectado. Longitud: {value.Length}");

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await ViewModel.ProcessCodeCommand.ExecuteAsync(value);
        });
    }

    private void OnZoomChanged(object sender, ValueChangedEventArgs e)
    {
        BarcodeReader.ZoomFactor = (float)e.NewValue;
    }

    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        try
        {
            // Evitar procesamientos simultáneos.
            if (ViewModel.IsBusy)
                return;

            // Pausar la cámara mientras se selecciona la imagen.
            BarcodeReader.IsDetecting = false;

            var photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo == null)
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

            if (qr == null || string.IsNullOrWhiteSpace(qr.Value))
            {
                await DisplayAlert(
                    "QR no encontrado",
                    "La imagen seleccionada no contiene un código QR válido.",
                    "Aceptar");

                return;
            }

            System.Diagnostics.Debug.WriteLine(
                "[INFESTCard][QR] Código detectado desde galería.");

            // Reutilizar exactamente la misma lógica de la cámara.
            await ViewModel.ProcessCodeCommand.ExecuteAsync(qr.Value);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[INFESTCard][QR] Error al procesar imagen: {ex}");

            await DisplayAlert(
                "Error",
                "No fue posible procesar la imagen seleccionada.",
                "Aceptar");
        }
        finally
        {
            // Reactivar la cámara si la página sigue visible.
            if (IsVisible && ViewModel.IsDetecting)
                BarcodeReader.IsDetecting = true;
        }
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
