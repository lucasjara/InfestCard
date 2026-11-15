using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CardDetailPage : ContentPage
{
    private bool hasPlayedEntrance;

    public CardDetailViewModel ViewModel =>
        (CardDetailViewModel)BindingContext;

    public CardDetailPage(CardDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (hasPlayedEntrance)
            return;

        hasPlayedEntrance = true;

        await PlayEntranceAnimationAsync();
    }


    private async Task PlayEntranceAnimationAsync()
    {
        // 1. Tarjeta e imagen simultáneamente.
        await Task.WhenAll(
            ShareableCard.FadeToAsync(1, 180, Easing.CubicOut),
            ShareableCard.TranslateToAsync(0, 0, 180, Easing.CubicOut),
            CardImage.FadeToAsync(1, 200, Easing.CubicOut),
            CardImage.ScaleToAsync(1, 220, Easing.CubicOut)
        );

        // 2. Información simultánea.
        await Task.WhenAll(
            FadeUpAsync(CardNameLabel, 120),
            FadeUpAsync(CategoryLabel, 120),
            CategoryDivider.FadeToAsync(1, 120, Easing.CubicOut),
            FadeUpAsync(DescriptionLabel, 140),
            FadeUpAsync(StatsGrid, 140)
        );

        // 3. Botones simultáneamente.
        await Task.WhenAll(
            FadeUpAsync(ActionsGrid, 120),
            FadeUpAsync(ShareButton, 150),
            FadeUpAsync(QrButton, 150),
            FadeUpAsync(DeleteButton, 150)
        );
    }


    private static async Task FadeUpAsync(
        VisualElement element,
        uint duration)
    {
        await Task.WhenAll(
            element.FadeToAsync(1, duration, Easing.CubicOut),
            element.TranslateToAsync(0, 0, duration, Easing.CubicOut)
        );
    }

    // Efecto de pulsación de los botones.
    private async void ActionButton_Pressed(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button)
            return;

        await button.ScaleToAsync(0.94, 70, Easing.CubicOut);
    }

    private async void ActionButton_Released(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button)
            return;

        await button.ScaleToAsync(1, 110, Easing.CubicOut);
    }

    // Compartir la carta como imagen.
    private async void ShareButton_Clicked(
        object sender,
        EventArgs e)
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

            await DisplayAlertAsync(
                "Error",
                "No se pudo compartir la carta.",
                "Aceptar");
        }
    }
}
