using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class ActivitiesViewModel : ObservableObject
{
    private readonly List<ActivityItem> _allActivities = [];

    [ObservableProperty]
    private ObservableCollection<ActivityItem> activities = [];

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
        "Presentación",
        "Concurso",
        "Charla",
        "Show",
        "Ceremonia"
    ];

    public ActivitiesViewModel()
    {
        LoadDefaultActivities();
        ApplyFilters();
    }

    private void LoadDefaultActivities()
    {
        var baseDate = DateTime.Today.AddDays(1);

        _allActivities.AddRange(
        [
            new ActivityItem
            {
                Id = 1,
                Title = "Ceremonia de Apertura",
                Category = "Ceremonia",
                Description = "Inauguración oficial de INFEST Chile 2026",
                StageName = "Escenario Principal",
                Location = "Escenario Principal · Zona Central",
                Icon = "🎉",
                StartAt = baseDate.AddHours(10),
                EndAt = baseDate.AddHours(11),
                Status = "scheduled"
            },
            new ActivityItem
            {
                Id = 2,
                Title = "Concurso Cosplay",
                Category = "Concurso",
                Description = "Gran concurso de cosplay con premios increíbles",
                StageName = "Escenario Principal",
                Location = "Escenario Principal · Zona Central",
                Icon = "🏆",
                StartAt = baseDate.AddHours(14),
                EndAt = baseDate.AddHours(16),
                Status = "scheduled"
            },
            new ActivityItem
            {
                Id = 3,
                Title = "Charla: Desarrollo Indie",
                Category = "Charla",
                Description = "Experiencias y consejos de desarrolladores independientes",
                StageName = "Escenario Tech",
                Location = "Escenario Tech · Zona Norte",
                Icon = "💡",
                StartAt = baseDate.AddHours(11),
                EndAt = baseDate.AddHours(12),
                Status = "scheduled"
            },
            new ActivityItem
            {
                Id = 4,
                Title = "Show de K-Pop",
                Category = "Show",
                Description = "Presentaciones de grupos de baile K-Pop",
                StageName = "Escenario Principal",
                Location = "Escenario Principal · Zona Central",
                Icon = "🎵",
                StartAt = baseDate.AddHours(16),
                EndAt = baseDate.AddHours(17).AddMinutes(30),
                Status = "scheduled"
            },
            new ActivityItem
            {
                Id = 5,
                Title = "Torneo: Fighting Games",
                Category = "Concurso",
                Description = "Torneo de Street Fighter 6 y Tekken 8",
                StageName = "Escenario Gaming",
                Location = "Escenario Gaming · Zona Sur",
                Icon = "👊",
                StartAt = baseDate.AddHours(12),
                EndAt = baseDate.AddHours(15),
                Status = "scheduled"
            },
            new ActivityItem
            {
                Id = 6,
                Title = "Presentación: Nuevo Juego Chileno",
                Category = "Presentación",
                Description = "Revelación exclusiva de proyecto nacional",
                StageName = "Escenario Tech",
                Location = "Escenario Tech · Zona Norte",
                Icon = "🚀",
                StartAt = baseDate.AddHours(15),
                EndAt = baseDate.AddHours(15).AddMinutes(45),
                Status = "scheduled"
            },
            new ActivityItem
            {
                Id = 7,
                Title = "Ceremonia de Clausura",
                Category = "Ceremonia",
                Description = "Entrega de premios y cierre del evento",
                StageName = "Escenario Principal",
                Location = "Escenario Principal · Zona Central",
                Icon = "🏁",
                StartAt = baseDate.AddDays(1).AddHours(18),
                EndAt = baseDate.AddDays(1).AddHours(19),
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
        var filtered = _allActivities.Where(activity =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                activity.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                activity.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                activity.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                activity.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                activity.StageName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                activity.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        })
        .OrderBy(a => a.StartAt)
        .ToList();

        Activities.Clear();

        foreach (var activity in filtered)
            Activities.Add(activity);

        HasResults = Activities.Count > 0;
        HasNoResults = !HasResults;
    }
}