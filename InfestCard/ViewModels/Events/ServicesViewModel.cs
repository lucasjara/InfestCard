using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class ServicesViewModel : BaseListViewModel<ServiceItem>
{
    public ServicesViewModel()
    {
        InitializeCategories("Salud", "Información", "Accesibilidad", "Seguridad", "Comida");
        LoadDefaultServices();
        ApplyFilters();
    }

    private void LoadDefaultServices()
    {
        AllItems.AddRange(
        [
            new ServiceItem
            {
                Id = 1,
                Name = "Primeros Auxilios",
                Slug = "primeros-auxilios",
                Category = "Salud",
                Description = "Servicio de atención médica.",
                Instructions = "Ubicado cerca del escenario principal.",
                Contact = "Cruz Roja",
                Location = "Zona A · Stand 01",
                Icon = "🚑",
                SortOrder = 1
            },
            new ServiceItem
            {
                Id = 2,
                Name = "Información",
                Slug = "informacion",
                Category = "Información",
                Description = "Punto de consulta general.",
                Instructions = "Personal capacitado para responder preguntas.",
                Contact = "Voluntarios",
                Location = "Zona Central · Stand 02",
                Icon = "ℹ️",
                SortOrder = 2
            },
            new ServiceItem
            {
                Id = 3,
                Name = "Baños",
                Slug = "banos",
                Category = "Accesibilidad",
                Description = "Instalaciones sanitarias.",
                Instructions = "Baños masculino, femenino y accesibles.",
                Contact = "Organización",
                Location = "Zona B · Sector 03",
                Icon = "🚻",
                SortOrder = 3
            },
            new ServiceItem
            {
                Id = 4,
                Name = "Guardarropía",
                Slug = "guardarropia",
                Category = "Seguridad",
                Description = "Servicio de custodia de pertenencias.",
                Instructions = "Entrega de ticket al dejar pertenencias.",
                Contact = "Seguridad",
                Location = "Zona C · Stand 04",
                Icon = "🧥",
                SortOrder = 4
            }
        ]);
    }

    protected override bool MatchesCategory(ServiceItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(ServiceItem item, string search)
        => item.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);

    protected override IEnumerable<ServiceItem> Order(IEnumerable<ServiceItem> items)
        => items.OrderBy(service => service.SortOrder);
}