using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.ViewModels;

public partial class CollectionViewModel : BaseViewModel
{
    private readonly ICardStorageService cardStorageService;
    private readonly IAppNavigationService appNavigationService;

    private string? selectedType;

    public IAsyncRelayCommand RefreshCommand { get; }

    public ObservableCollection<Card> Cards { get; } = [];
    public ObservableCollection<Card> FilteredCards { get; } = [];

    public ObservableCollection<CardTypeFilter> AvailableTypes { get; } = [];

    public string? SelectedType
    {
        get => selectedType;
        set
        {
            if (SetProperty(ref selectedType, value))
                ApplyFilters();
        }
    }

    public bool IsEmpty => Cards.Count == 0;

    public CollectionViewModel(
        ICardStorageService cardStorageService,
        IAppNavigationService appNavigationService)
    {
        this.cardStorageService = cardStorageService;
        this.appNavigationService = appNavigationService;

        RefreshCommand = new AsyncRelayCommand(LoadAsync);

        _ = LoadAsync();
    }

    [RelayCommand]
    private Task NavigateToScannerAsync() =>
        appNavigationService.NavigateToScannerAsync();

    [RelayCommand]
    private Task SelectCardAsync(Card? card) => card is null
        ? Task.CompletedTask
        : appNavigationService.NavigateToCardDetailAsync(card);

    [RelayCommand]
    private void SelectType(string? type)
    {
        SelectedType = type;
    }

    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var cards = await cardStorageService.GetCardsAsync();

            Cards.Clear();

            foreach (var card in cards)
                Cards.Add(card);

            UpdateAvailableTypes();
            ApplyFilters();

            OnPropertyChanged(nameof(IsEmpty));
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

    private void UpdateAvailableTypes()
    {
        var types = Cards
            .Select(card => card.Type)
            .Where(type => !string.IsNullOrWhiteSpace(type))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(type => type)
            .ToList();

        AvailableTypes.Clear();

        foreach (var type in types)
        {
            var filter = type.ToLowerInvariant() switch
            {
                "pokemon" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Pokémon",
                    Icon = "filter_pokemon.svg"
                },

                "personal" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Personal",
                    Icon = "filter_personal.svg"
                },

                "default" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Predeterminadas",
                    Icon = "filter_default.svg"
                },

                _ => new CardTypeFilter
                {
                    Type = type,
                    Label = type,
                    Icon = "filter_default.svg"
                }
            };

            AvailableTypes.Add(filter);
        }

        if (SelectedType != null &&
            !AvailableTypes.Any(x =>
                string.Equals(x.Type, SelectedType,
                    StringComparison.OrdinalIgnoreCase)))
        {
            SelectedType = null;
        }
    }

    private static string GetTypeLabel(string? type)
    {
        return type?.ToLowerInvariant() switch
        {
            "pokemon" => "Pokémon",
            "personal" => "Personal",
            "default" => "Predeterminadas",
            _ => type ?? string.Empty
        };
    }
    private void ApplyFilters()
    {
        var filtered = Cards.Where(card =>
            SelectedType == null ||
            string.Equals(
                card.Type,
                SelectedType,
                StringComparison.OrdinalIgnoreCase
            ));

        FilteredCards.Clear();

        foreach (var card in filtered)
            FilteredCards.Add(card);
    }
}
