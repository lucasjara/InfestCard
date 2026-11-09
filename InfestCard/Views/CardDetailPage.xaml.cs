using InfestCard.Services.Interfaces;
using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CardDetailPage : ContentPage
{
    public CardDetailViewModel ViewModel => (CardDetailViewModel)BindingContext;

    public CardDetailPage(CardDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private async void ShareButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var screenshot = await ShareableCard.CaptureAsync();

            if (screenshot is null)
                return;

            var filePath = Path.Combine(
                FileSystem.CacheDirectory,
                $"card-{Guid.NewGuid():N}.png");

            await using (var stream = File.Create(filePath))
            {
                await screenshot.CopyToAsync(stream);
            }

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir carta",
                File = new ShareFile(filePath, "image/png")
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlert(
                "Error",
                "No se pudo compartir la carta.",
                "Aceptar");
        }
    }
}
