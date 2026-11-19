using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class SponsorsViewModel : BaseListViewModel<SponsorItem>
{
    public SponsorsViewModel()
    {
        InitializeCategories("Oro", "Plata", "Bronce", "Media", "Local");
        LoadDefaultSponsors();
        ApplyFilters();
    }

    private void LoadDefaultSponsors()
    {
        AllItems.AddRange(
        [
            new SponsorItem
            {
                Id = 1,
                Name = "TechStore",
                Slug = "techstore",
                Category = "Oro",
                Description = "Mayorista y minorista de tecnología.",
                Website = "https://techstore.cl",
                Location = "Zona A · Stand 01",
                Icon = "💻",
                SortOrder = 1
            },
            new SponsorItem
            {
                Id = 2,
                Name = "GamingChile",
                Slug = "gamingchile",
                Category = "Plata",
                Description = "Comunidad de gaming y esports.",
                Website = "https://gamingchile.cl",
                Location = "Zona B · Stand 03",
                Icon = "🎮",
                SortOrder = 2
            },
            new SponsorItem
            {
                Id = 3,
                Name = "AnimeStore",
                Slug = "animestore",
                Category = "Bronce",
                Description = "Tienda de productos de anime y manga.",
                Website = "https://animestore.cl",
                Location = "Zona C · Stand 05",
                Icon = "★",
                SortOrder = 3
            },
            new SponsorItem
            {
                Id = 4,
                Name = "RetroGaming",
                Slug = "retrogaming",
                Category = "Media",
                Description = "Consolas y videojuegos retro.",
                Website = "https://retrogaming.cl",
                Location = "Zona D · Stand 07",
                Icon = "🎛️",
                SortOrder = 4
            }
        ]);
    }

    protected override bool MatchesCategory(SponsorItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(SponsorItem item, string search)
        => item.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);

    protected override IEnumerable<SponsorItem> Order(IEnumerable<SponsorItem> items)
        => items.OrderBy(sponsor => sponsor.SortOrder);
}