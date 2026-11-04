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
    }

    private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs eventArgs)
    {
        var value = eventArgs.Results.FirstOrDefault()?.Value;
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await ViewModel.ProcessCodeCommand.ExecuteAsync(value);
        });
    }
}
