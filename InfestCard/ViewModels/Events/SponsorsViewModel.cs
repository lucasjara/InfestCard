using InfestCard.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfestCard.ViewModels.Events;

public partial class SponsorsViewModel : ObservableObject
{
    private readonly List<SponsorItem> _allSponsors = new();

    [ObservableProperty]
    private ObservableCollection<SponsorItem> sponsors = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedCategory = "Todos";

    [ObservableProperty]
    private bool hasResults;

    [ObservableProperty]
    private bool hasNoResults;

    public ObservableCollection<string> Categories { get; } = new()
    {
        "Todos",
        "Oro",
        "Plata",
        "Bronce",
        "Media",
        "Local"
    };

    public SponsorsViewModel()
    {
        LoadDefaultSponsors();
        ApplyFilters();
    }

    private void LoadDefaultSponsors()
    {
        _allSponsors.AddRange(new[]
        {
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
        });
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
        var filtered = _allSponsors.Where(sponsor =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                sponsor.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                sponsor.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                sponsor.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                sponsor.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                sponsor.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        })
        .OrderBy(s => s.SortOrder)
        .ToList();

        Sponsors.Clear();

        foreach (var sponsor in filtered)
            Sponsors.Add(sponsor);

        HasResults = Sponsors.Count > 0;
        HasNoResults = !HasResults;
    }
}