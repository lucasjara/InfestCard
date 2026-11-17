using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class CosplayViewModel : ObservableObject
{
    private readonly List<CosplayItem> _allActivities = [];

    [ObservableProperty]
    private ObservableCollection<CosplayItem> activities = [];

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
        "Concurso",
        "Desfile",
        "Exhibición",
        "Performance"
    ];

    public CosplayViewModel()
    {
        LoadDefaultActivities();
        ApplyFilters();
    }

    private void LoadDefaultActivities()
    {
        var baseDate = DateTime.Today.AddDays(1);

        _allActivities.AddRange(
        [
            new CosplayItem
            {
                Id = 1,
                Title = "Concurso Cosplay Individual",
                Category = "Concurso",
                Description = "Competencia individual con evaluación de confección y caracterización.",
                Rules = "Inscripción previa en mesa de información. Tiempo máximo en escenario: 2 minutos.",
                Participants = "Categorías: Novato, Avanzado, Experto",
                Location = "Escenario Principal · Zona Central",
                StartAt = baseDate.AddHours(14),
                EndAt = baseDate.AddHours(16),
                Status = "scheduled"
            },
            new CosplayItem
            {
                Id = 2,
                Title = "Desfile Cosplay Grupal",
                Category = "Desfile",
                Description = "Presentación de grupos con temáticas de series, juegos y películas.",
                Rules = "Grupos de 3 a 10 participantes. Música enviada con anticipación.",
                Participants = "Inscripción por equipos",
                Location = "Escenario Principal · Zona Central",
                StartAt = baseDate.AddHours(12),
                EndAt = baseDate.AddHours(13),
                Status = "scheduled"
            },
            new CosplayItem
            {
                Id = 3,
                Title = "Exhibición de Armaduras y Props",
                Category = "Exhibición",
                Description = "Muestra de trabajos destacados en armaduras, armas y utilería.",
                Rules = "Props deben cumplir normativa de seguridad del evento.",
                Location = "Sala Cosplay · Zona Sur",
                StartAt = baseDate.AddHours(11),
                EndAt = baseDate.AddHours(12),
                Status = "scheduled"
            },
            new CosplayItem
            {
                Id = 4,
                Title = "Performance Cosplay",
                Category = "Performance",
                Description = "Escenas y coreografías interpretadas por cosplayers.",
                Rules = "Duración máxima 3 minutos por presentación.",
                Participants = "Individual o grupal",
                Location = "Escenario Principal · Zona Central",
                StartAt = baseDate.AddDays(1).AddHours(15),
                EndAt = baseDate.AddDays(1).AddHours(16).AddMinutes(30),
                Status = "scheduled"
            },
            new CosplayItem
            {
                Id = 5,
                Title = "Cosplay Infantil",
                Category = "Desfile",
                Description = "Desfile y premiación para los más pequeños.",
                Rules = "Participantes menores de 12 años acompañados por un adulto.",
                Location = "Escenario Secundario · Zona Norte",
                StartAt = baseDate.AddDays(1).AddHours(12),
                EndAt = baseDate.AddDays(1).AddHours(13),
                Status = "scheduled"
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
        var filtered = _allActivities.Where(item =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                item.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                item.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                item.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                item.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                item.Rules.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                item.Participants.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                item.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        })
        .OrderBy(a => a.StartAt)
        .ToList();

        Activities.Clear();

        foreach (var item in filtered)
            Activities.Add(item);

        HasResults = Activities.Count > 0;
        HasNoResults = !HasResults;
    }
}