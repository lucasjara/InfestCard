using InfestCard.Models;

namespace InfestCard.ViewModels;

public sealed class CardDetailViewModel : BaseViewModel
{
    public Card? Card { get; private set; }

    public string LevelIndicators => Card is null || Card.Level <= 0
        ? string.Empty
        : string.Join(" ", Enumerable.Repeat("◆", Card.Level));

    public string ImageSource => Card?.LocalImagePath ?? Card?.ImageUrl ?? string.Empty;

    public void Load(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);
        Card = card;
        OnPropertyChanged(nameof(Card));
        OnPropertyChanged(nameof(LevelIndicators));
        OnPropertyChanged(nameof(ImageSource));
    }
}
