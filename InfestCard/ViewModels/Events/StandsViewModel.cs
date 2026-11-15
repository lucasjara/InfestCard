using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class StandsViewModel : ObservableObject
{
    private readonly List<StandItem> _allStands = [];

    [ObservableProperty]
    private ObservableCollection<StandItem> stands = [];

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedCategory = "Todos";

    [ObservableProperty]
    private bool hasResults;
    [ObservableProperty]
    private bool hasNoResults;

    public ObservableCollection<string> Categories { get; } =
    [
        "Todos",
        "Gaming",
        "Tecnología",
        "Comida",
        "Comercio"
    ];

    public StandsViewModel()
    {
        LoadDefaultStands();
        ApplyFilters();
    }

    private void LoadDefaultStands()
    {
        _allStands.AddRange(
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

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        ApplyFilters();
    }

    public void SelectCategory(string category)
    {
        SelectedCategory = category;
    }

    private void ApplyFilters()
    {
        var filtered = _allStands.Where(stand =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                stand.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                stand.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                stand.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                stand.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                stand.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        }).ToList();

        Stands.Clear();

        foreach (var stand in filtered)
            Stands.Add(stand);

        HasResults = Stands.Count > 0;
        HasNoResults = !HasResults;
    }
}
