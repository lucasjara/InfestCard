using System.Text.Json;
using InfestCard.Models;

namespace InfestCard.Views;

public partial class QrCodePage : ContentPage
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public QrCodePage()
    {
        InitializeComponent();
    }

    public void Load(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);

        var payload = JsonSerializer.Serialize(new
        {
            card = new
            {
                card.Id,
                card.Type,
                card.Name,
                card.Description,
                card.Category,
                card.Level,
                card.Image,
                card.ImageUrl,
                card.CreatedAt
            },
            quantity = card.Quantity
        }, SerializerOptions);

        QrGenerator.Value = payload;
        CardNameLabel.Text = card.Name;
    }
}