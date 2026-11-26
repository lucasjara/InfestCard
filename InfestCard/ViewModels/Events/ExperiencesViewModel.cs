using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class ExperiencesViewModel : BaseListViewModel<ExperienceItem>
{
    public ExperiencesViewModel()
    {
        InitializeCategories("Inmersiva", "VR", "Gaming", "Realidad Aumentada");
        LoadDefaultExperiences();
        ApplyFilters();
    }

    private void LoadDefaultExperiences()
    {
        AllItems.AddRange(
        [
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
        ]);
    }

    protected override bool MatchesCategory(ExperienceItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(ExperienceItem item, string search)
        => item.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);
}