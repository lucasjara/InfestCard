using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class CosplayViewModel : BaseListViewModel<CosplayItem>
{
    public CosplayViewModel()
    {
        InitializeCategories("Concurso", "Desfile", "Exhibición", "Performance");
        LoadDefaultActivities();
        ApplyFilters();
    }

    private void LoadDefaultActivities()
    {
        var baseDate = DateTime.Today.AddDays(1);

        AllItems.AddRange(
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

    protected override bool MatchesCategory(CosplayItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(CosplayItem item, string search)
        => item.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Rules.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Participants.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);

    protected override IEnumerable<CosplayItem> Order(IEnumerable<CosplayItem> items)
        => items.OrderBy(item => item.StartAt);
}