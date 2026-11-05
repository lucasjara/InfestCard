namespace InfestCard.Models;

public sealed class CardApiResponse
{
    public Card? Card { get; init; }

    public CardCollection? Collection { get; init; }

    public int Quantity { get; init; }
}
