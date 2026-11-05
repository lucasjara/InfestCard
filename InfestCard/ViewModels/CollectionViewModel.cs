using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.ViewModels;

public partial class CollectionViewModel : BaseViewModel
{
    private readonly ICardStorageService cardStorageService;
    private readonly IAppNavigationService appNavigationService;

    public IAsyncRelayCommand RefreshCommand { get; }

    public CollectionViewModel(
        ICardStorageService cardStorageService,
        IAppNavigationService appNavigationService)
    {
        this.cardStorageService = cardStorageService;
        this.appNavigationService = appNavigationService;
        RefreshCommand = new AsyncRelayCommand(LoadAsync);
        Cards.CollectionChanged += (_, _) => OnPropertyChanged(nameof(IsEmpty));
        _ = LoadAsync();
    }

    public ObservableCollection<Card> Cards { get; } = [];

    public bool IsEmpty => Cards.Count == 0;

    [RelayCommand]
    private Task NavigateToScannerAsync() => appNavigationService.NavigateToScannerAsync();

    [RelayCommand]
    private Task SelectCardAsync(Card? card) => card is null
        ? Task.CompletedTask
        : appNavigationService.NavigateToCardDetailAsync(card);

    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var cards = await cardStorageService.GetCardsAsync();
            Cards.Clear();

            foreach (var card in cards)
            {
                Cards.Add(card);
            }
        }
        catch (Exception exception)
        {
            ErrorMessage = "No se pudo cargar la colección local.";
            System.Diagnostics.Debug.WriteLine(exception);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
