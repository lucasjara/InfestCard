using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class ActivitiesViewModel : BaseListViewModel<ActivityItem>
{
    public ActivitiesViewModel()
    {
        InitializeCategories("Presentación", "Concurso", "Charla", "Show", "Ceremonia");
        LoadDefaultActivities();
        ApplyFilters();
    }

    private void LoadDefaultActivities()
    {
        var baseDate = DateTime.Today.AddDays(1);

        AllItems.AddRange(
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

    protected override bool MatchesCategory(ActivityItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(ActivityItem item, string search)
        => item.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.StageName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);

    protected override IEnumerable<ActivityItem> Order(IEnumerable<ActivityItem> items)
        => items.OrderBy(activity => activity.StartAt);
}