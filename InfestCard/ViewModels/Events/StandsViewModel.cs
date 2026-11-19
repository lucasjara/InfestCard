using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class StandsViewModel : BaseListViewModel<StandItem>
{
    public StandsViewModel()
    {
        InitializeCategories("Gaming", "Tecnología", "Comida", "Comercio");
        LoadDefaultStands();
        ApplyFilters();
    }

    private void LoadDefaultStands()
    {
        AllItems.AddRange(
        [
            new StandItem
            {
                Id = 1,
                Name = "Gaming Zone",
                Category = "Gaming",
                Description = "Zona de videojuegos, consolas y entretenimiento.",
                Location = "Zona A · Stand 01",
                Icon = "🎮"
            },
            new StandItem
            {
                Id = 2,
                Name = "Tech Store",
                Category = "Tecnología",
                Description = "Accesorios, periféricos y productos tecnológicos.",
                Location = "Zona A · Stand 02",
                Icon = "💻"
            },
            new StandItem
            {
                Id = 3,
                Name = "Retro Gaming",
                Category = "Gaming",
                Description = "Videojuegos clásicos y experiencias retro.",
                Location = "Zona B · Stand 03",
                Icon = "🕹️"
            },
            new StandItem
            {
                Id = 4,
                Name = "Food Corner",
                Category = "Comida",
                Description = "Espacio de alimentación y snacks.",
                Location = "Zona C · Stand 04",
                Icon = "🍔"
            },
            new StandItem
            {
                Id = 5,
                Name = "Anime Store",
                Category = "Comercio",
                Description = "Productos, coleccionables y artículos de anime.",
                Location = "Zona B · Stand 05",
                Icon = "🎌"
            },
            new StandItem
            {
                Id = 6,
                Name = "PC Gaming",
                Category = "Tecnología",
                Description = "Equipamiento y demostraciones de PC gaming.",
                Location = "Zona A · Stand 06",
                Icon = "🖥️"
            }
        ]);
    }

    protected override bool MatchesCategory(StandItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(StandItem item, string search)
        => item.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);
}