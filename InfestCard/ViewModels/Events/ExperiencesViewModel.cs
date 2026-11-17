using InfestCard.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfestCard.ViewModels.Events;

public partial class ExperiencesViewModel : ObservableObject
{
    private readonly List<ExperienceItem> _allExperiences = new();

    [ObservableProperty]
    private ObservableCollection<ExperienceItem> experiences = new();

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
        "Inmersiva",
        "VR",
        "Gaming",
        "Realidad Aumentada"
    };

    public ExperiencesViewModel()
    {
        LoadDefaultExperiences();
        ApplyFilters();
    }

    private void LoadDefaultExperiences()
    {
        _allExperiences.AddRange(new[]
        {
            new ExperienceItem
            {
                Id = 1,
                Name = "VR Zone",
                Category = "VR",
                Description = "Experiencia de realidad virtual con los últimos equipos.",
                Location = "Zona A · Stand 01",
                Icon = "🥽"
            },
            new ExperienceItem
            {
                Id = 2,
                Name = "Retro Gaming",
                Category = "Gaming",
                Description = "Consolas clásicas y videojuegos retro para jugar.",
                Location = "Zona B · Stand 03",
                Icon = "🎛️"
            },
            new ExperienceItem
            {
                Id = 3,
                Name = "AR Art Gallery",
                Category = "Realidad Aumentada",
                Description = "Galería de arte interactiva con realidad aumentada.",
                Location = "Zona C · Stand 05",
                Icon = "🎨"
            },
            new ExperienceItem
            {
                Id = 4,
                Name = "Inmersive Cinema",
                Category = "Inmersiva",
                Description = "Cine inmersivo con pantalla 360° y audio espacial.",
                Location = "Zona D · Sala 1",
                Icon = "🍿"
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
        var filtered = _allExperiences.Where(exp =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                exp.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                exp.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                exp.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                exp.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                exp.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        }).ToList();

        Experiences.Clear();

        foreach (var exp in filtered)
            Experiences.Add(exp);

        HasResults = Experiences.Count > 0;
        HasNoResults = !HasResults;
    }
}