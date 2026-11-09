
using CommunityToolkit.Mvvm.Input;
using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.ViewModels;

public sealed partial class CardDetailViewModel : BaseViewModel
{
    private readonly ICardStorageService cardStorageService;
    private readonly IAppNavigationService appNavigationService;
    private readonly ICardShareService cardShareService;

    public Card? Card { get; private set; }

    public CardDetailViewModel(
    ICardStorageService cardStorageService,
    IAppNavigationService appNavigationService,
    ICardShareService cardShareService)
    {
        this.cardStorageService = cardStorageService;
        this.appNavigationService = appNavigationService;
        this.cardShareService = cardShareService;
    }

    public string LevelIndicators => Card is null || Card.Level <= 0
        ? string.Empty
        : string.Join(" ", Enumerable.Repeat("◆", Math.Min(Card.Level, 10)));

    public string ImageSource =>
        Card?.LocalImagePath ?? Card?.ImageUrl ?? "card_placeholder.svg";

    public string CategoryDisplayName => Card?.Category?.ToLowerInvariant() switch
    {
        "fire" => "FUEGO",
        "water" => "AGUA",
        "grass" => "PLANTA",
        "electric" => "ELÉCTRICO",
        "ice" => "HIELO",
        "fighting" => "LUCHA",
        "poison" => "VENENO",
        "ground" => "TIERRA",
        "flying" => "VOLADOR",
        "psychic" => "PSÍQUICO",
        "bug" => "BICHO",
        "rock" => "ROCA",
        "ghost" => "FANTASMA",
        "dragon" => "DRAGÓN",
        "dark" => "OSCURIDAD",
        "steel" => "ACERO",
        "fairy" => "HADA",
        "normal" => "NORMAL",
        "nature" => "NATURALEZA",
        "magic" => "MÁGICO",
        _ => Card?.Category?.ToUpperInvariant() ?? "GENERAL"
    };

    public void Load(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);

        Card = card;

        OnPropertyChanged(nameof(Card));
        OnPropertyChanged(nameof(LevelIndicators));
        OnPropertyChanged(nameof(CategoryDisplayName));
        OnPropertyChanged(nameof(ImageSource));
    }

    [RelayCommand]
    private async Task ShareCardAsync(byte[] imageBytes)
    {
        if (Card is null || imageBytes is null || imageBytes.Length == 0)
            return;

        try
        {
            await cardShareService.ShareAsync(
                imageBytes,
                $"Compartir {Card.Name}");
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);

            await Shell.Current.DisplayAlert(
                "Error",
                "No se pudo compartir la carta.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Card is null)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Eliminar carta",
            $"¿Estás seguro de eliminar '{Card.Name}' de tu colección?",
            "Eliminar",
            "Cancelar");

        if (!confirm)
            return;

        try
        {
            await cardStorageService.DeleteCardAsync(Card.Id);

            await appNavigationService.NavigateBackToCollectionAsync();
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);

            await Shell.Current.DisplayAlert(
                "Error",
                "No se pudo eliminar la carta.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private async Task GenerateQrAsync()
    {
        if (Card is null)
            return;

        try
        {
            await appNavigationService.NavigateToQrAsync(Card);
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);

            await Shell.Current.DisplayAlert(
                "Error",
                "No se pudo generar el código QR.",
                "Aceptar");
        }
    }
}
