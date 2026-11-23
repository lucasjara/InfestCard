
using System.Runtime.CompilerServices;
using InfestCard.Models;
using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CollectionPage : ContentPage
{
    private readonly ConditionalWeakTable<Border, object> animatedCards = new();

    private int cardAnimationIndex;
    private bool hasPlayedEntrance;

    public CollectionPage(CollectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private CollectionViewModel ViewModel =>
        (CollectionViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            if (!hasPlayedEntrance)
            {
                hasPlayedEntrance = true;
                await AnimatePageEntranceAsync();
            }

            await ViewModel.RefreshCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[INFESTCard][CollectionPage] Error al refrescar la colección: {ex.Message}");

            await DisplayAlertAsync(
                "Error",
                "No se pudo actualizar la colección. Por favor, inténtalo de nuevo más tarde.",
                "OK");
        }
    }

    private async Task AnimatePageEntranceAsync()
    {
        var headerAnimation = Task.WhenAll(
            HeaderView.FadeToAsync(1, 300, Easing.CubicOut),
            HeaderView.TranslateToAsync(0, 0, 300, Easing.CubicOut)
        );

        var scannerAnimation = AnimateScannerButtonAsync();

        await Task.WhenAll(
            headerAnimation,
            scannerAnimation
        );
    }

    private async Task AnimateScannerButtonAsync()
    {
        await Task.Delay(100);

        await ScannerButton.ScaleToAsync(
            1,
            280,
            Easing.SpringOut
        );
    }

    private async void OnCardLoaded(object? sender, EventArgs e)
    {
        if (sender is not Border cardView)
            return;

        if (animatedCards.TryGetValue(cardView, out _))
            return;

        animatedCards.Add(cardView, new object());

        int animationOrder = cardAnimationIndex++ % 6;

        await Task.Delay(animationOrder * 45);

        await Task.WhenAll(
            cardView.FadeToAsync(1, 250, Easing.CubicOut),
            cardView.TranslateToAsync(0, 0, 250, Easing.CubicOut)
        );
    }

    private async void OnCardSelected(
        object? sender,
        SelectionChangedEventArgs eventArgs)
    {
        if (eventArgs.CurrentSelection.FirstOrDefault() is not Card card)
            return;

        CardsView.SelectedItem = null;

        await ViewModel.SelectCardCommand.ExecuteAsync(card);
    }

    private async void OnOpenFiltersTapped(
        object? sender,
        TappedEventArgs eventArgs)
    {
        try
        {
            var filtersPage = new CollectionFiltersPage(ViewModel);
            NavigationPage.SetHasNavigationBar(filtersPage, false);
            await Navigation.PushModalAsync(
                new NavigationPage(filtersPage)
                {
                    BarBackgroundColor = Color.FromArgb("#151520"),
                    BarTextColor = Colors.White
                });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[INFESTCard][CollectionPage] Error al abrir filtros: {ex.Message}");

            await DisplayAlertAsync(
                "Error",
                "No se pudo abrir el selector de filtros.",
                "OK");
        }
    }

}
