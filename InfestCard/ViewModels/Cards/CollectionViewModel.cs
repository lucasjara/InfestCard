using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.ViewModels;

public partial class CollectionViewModel : BaseViewModel
{
    private readonly ICardStorageService cardStorageService;
    private readonly IAppNavigationService appNavigationService;

    private readonly HashSet<string> selectedTypes =
        new(StringComparer.OrdinalIgnoreCase);

    public IAsyncRelayCommand RefreshCommand { get; }

    public ObservableCollection<Card> Cards { get; } = [];
    public ObservableCollection<Card> FilteredCards { get; } = [];

    public ObservableCollection<CardTypeFilter> AvailableTypes { get; } = [];

    public IReadOnlyCollection<string> SelectedTypes => selectedTypes;

    public string SelectedTypeLabel
    {
        get
        {
            if (selectedTypes.Count == 0)
                return "Todas las categorías";

            if (selectedTypes.Count == 1)
            {
                var selected = AvailableTypes.FirstOrDefault(
                    x => selectedTypes.Contains(x.Type));

                return selected?.Label ?? "1 categoría seleccionada";
            }

            return $"{selectedTypes.Count} categorías seleccionadas";
        }
    }

    public bool HasActiveFilters => selectedTypes.Count > 0;

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
    private Task SelectCardAsync(Card? card) =>
        card is null
            ? Task.CompletedTask
            : appNavigationService.NavigateToCardDetailAsync(card);

    [RelayCommand]
    private void ClearFilters()
    {
        selectedTypes.Clear();

        UpdateSelectedFilter();
        ApplyFilters();
    }

    public void ApplyTypeFilters(IEnumerable<string> types)
    {
        selectedTypes.Clear();

        foreach (var type in types.Where(
                     x => !string.IsNullOrWhiteSpace(x)))
        {
            selectedTypes.Add(type);
        }

        // Evita conservar categorías que ya no existen.
        selectedTypes.RemoveWhere(type =>
            !AvailableTypes.Any(x =>
                string.Equals(
                    x.Type,
                    type,
                    StringComparison.OrdinalIgnoreCase)));

        UpdateSelectedFilter();
        ApplyFilters();
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
            UpdateSelectedFilter();
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
                "starwars" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Star Wars",
                    Icon = "filter_starwars.svg"
                },

                "minecraft" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Minecraft",
                    Icon = "filter_minecraft.svg"
                },

                "dragonball" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Dragon Ball",
                    Icon = "filter_dragonball.svg"
                },

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
                    Icon = "card_personalizada.svg"
                },

                "default" => new CardTypeFilter
                {
                    Type = type,
                    Label = "Predeterminadas",
                    Icon = "card_predefinida.svg"
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

        // Si una categoría desapareció de la colección,
        // deja de estar seleccionada.
        selectedTypes.RemoveWhere(type =>
            !AvailableTypes.Any(x =>
                string.Equals(
                    x.Type,
                    type,
                    StringComparison.OrdinalIgnoreCase)));
    }

    private void UpdateSelectedFilter()
    {
        foreach (var filter in AvailableTypes)
        {
            filter.IsSelected = selectedTypes.Contains(filter.Type);
        }

        OnPropertyChanged(nameof(SelectedTypes));
        OnPropertyChanged(nameof(SelectedTypeLabel));
        OnPropertyChanged(nameof(HasActiveFilters));
    }

    private void ApplyFilters()
    {
        var filtered = Cards.Where(card =>
            selectedTypes.Count == 0 ||
            selectedTypes.Contains(card.Type));

        FilteredCards.Clear();

        foreach (var card in filtered)
            FilteredCards.Add(card);
    }
}
