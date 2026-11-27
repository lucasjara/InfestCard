using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfestCard.Views;
using InfestCard.Views.Events;

namespace InfestCard.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IServiceProvider _services;

    public HomeViewModel(IServiceProvider services)
    {
        _services = services;
    }

    [RelayCommand]
    private async Task NavigateToCardsAsync()
    {
        var page = _services.GetRequiredService<CollectionPage>();

        NavigationPage.SetHasNavigationBar(page, false);

        await Application.Current!.Windows[0]
            .Page!.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task NavigateToGuideAsync()
    {
        var page = _services.GetRequiredService<ExplorerMapPage>();

        NavigationPage.SetHasNavigationBar(page, false);

        await Application.Current!.Windows[0]
            .Page!.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task NavigateToPassportAsync()
    {
        await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
            "Pasaporte INFEST",
            "Este módulo estará disponible próximamente.",
            "Aceptar");
    }
}