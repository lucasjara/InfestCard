using InfestCard.Models;

namespace InfestCard.ViewModels;

public sealed class CardDetailViewModel : BaseViewModel
{
    public Card? Card { get; private set; }

    public string LevelIndicators => Card is null || Card.Level <= 0
        ? string.Empty
        : string.Join(" ", Enumerable.Repeat("◆", Card.Level));

    public string ImageSource => Card?.LocalImagePath ?? Card?.ImageUrl ?? "card_placeholder.svg";
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

        // Categorías personalizadas de INFEST
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
}
