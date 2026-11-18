using InfestCard.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfestCard.ViewModels.Events;

public partial class ServicesViewModel : ObservableObject
{
    private readonly List<ServiceItem> _allServices = new();

    [ObservableProperty]
    private ObservableCollection<ServiceItem> services = new();

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
        "Salud",
        "Información",
        "Accesibilidad",
        "Seguridad",
        "Comida"
    };

    public ServicesViewModel()
    {
        LoadDefaultServices();
        ApplyFilters();
    }

    private void LoadDefaultServices()
    {
        _allServices.AddRange(new[]
        {
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
        var filtered = _allServices.Where(service =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                service.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                service.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                service.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                service.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                service.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        })
        .OrderBy(s => s.SortOrder)
        .ToList();

        Services.Clear();

        foreach (var service in filtered)
            Services.Add(service);

        HasResults = Services.Count > 0;
        HasNoResults = !HasResults;
    }
}