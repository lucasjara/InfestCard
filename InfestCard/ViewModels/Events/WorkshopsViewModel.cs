using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class WorkshopsViewModel : BaseListViewModel<WorkshopItem>
{
    public WorkshopsViewModel()
    {
        InitializeCategories("Arte", "Tecnología", "Cosplay", "Gaming", "Manualidades");
        LoadDefaultWorkshops();
        ApplyFilters();
    }

    private void LoadDefaultWorkshops()
    {
        var baseDate = DateTime.Today.AddDays(1);

        AllItems.AddRange(
        [
            new WorkshopItem
            {
                Id = 1,
                Title = "Introducción a impresión 3D",
                Instructor = "Equipo MakerLab",
                Category = "Tecnología",
                Description = "Aprende los fundamentos del modelado e impresión 3D.",
                Requirements = "Traer notebook (opcional)",
                Capacity = 20,
                Location = "Sala de talleres · Zona Norte",
                Icon = "🖨️",
                StartAt = baseDate.AddHours(10).AddMinutes(30),
                EndAt = baseDate.AddHours(11).AddMinutes(30),
                Status = "scheduled"
            },
            new WorkshopItem
            {
                Id = 2,
                Title = "Dibujo de personajes anime",
                Instructor = "Camila Rojas",
                Category = "Arte",
                Description = "Técnicas básicas para crear y estilizar personajes.",
                Requirements = "Lápiz grafito y block de dibujo",
                Capacity = 25,
                Location = "Sala creativa · Zona Central",
                Icon = "🎨",
                StartAt = baseDate.AddHours(12),
                EndAt = baseDate.AddHours(13).AddMinutes(30),
                Status = "scheduled"
            },
            new WorkshopItem
            {
                Id = 3,
                Title = "Armado de props con EVA foam",
                Instructor = "Taller CosPro",
                Category = "Cosplay",
                Description = "Corte, termoformado y pintura de props para cosplay.",
                Requirements = "Materiales incluidos",
                Capacity = 15,
                Location = "Sala cosplay · Zona Sur",
                Icon = "⚔️",
                StartAt = baseDate.AddHours(14),
                EndAt = baseDate.AddHours(16),
                Status = "scheduled"
            },
            new WorkshopItem
            {
                Id = 4,
                Title = "Creación de niveles en Unity",
                Instructor = "Dev Indie Chile",
                Category = "Gaming",
                Description = "Diseño y prototipado rápido de niveles para videojuegos.",
                Requirements = "Conocimientos básicos de Unity",
                Capacity = 18,
                Location = "Sala tech · Zona Norte",
                Icon = "🎮",
                StartAt = baseDate.AddHours(11),
                EndAt = baseDate.AddHours(12).AddMinutes(30),
                Status = "scheduled"
            },
            new WorkshopItem
            {
                Id = 5,
                Title = "Llaveros y pins personalizados",
                Instructor = "Craft Studio",
                Category = "Manualidades",
                Description = "Crea tus propios accesorios personalizados del evento.",
                Requirements = "Sin requisitos previos",
                Capacity = 30,
                Location = "Sala creativa · Zona Central",
                Icon = "📿",
                StartAt = baseDate.AddDays(1).AddHours(11),
                EndAt = baseDate.AddDays(1).AddHours(12),
                Status = "scheduled"
            }
        ]);
    }

    protected override bool MatchesCategory(WorkshopItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(WorkshopItem item, string search)
        => item.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Instructor.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Requirements.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);

    protected override IEnumerable<WorkshopItem> Order(IEnumerable<WorkshopItem> items)
        => items.OrderBy(workshop => workshop.StartAt);
}