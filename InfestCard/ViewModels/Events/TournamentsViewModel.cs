using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class TournamentsViewModel : ObservableObject
{
    private readonly List<TournamentItem> _allTournaments = [];

    [ObservableProperty]
    private ObservableCollection<TournamentItem> tournaments = [];

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
        "Fighting",
        "TCG",
        "Shooter",
        "MOBA",
        "Sports"
    ];

    public TournamentsViewModel()
    {
        LoadDefaultTournaments();
        ApplyFilters();
    }

    private void LoadDefaultTournaments()
    {
        var baseDate = DateTime.Today.AddDays(1);

        _allTournaments.AddRange(
        [
            new TournamentItem
            {
                Id = 1,
                Name = "Street Fighter 6 Championship",
                Game = "Street Fighter 6",
                Category = "Fighting",
                Description = "Torneo oficial con brackets de eliminación doble.",
                Format = "Eliminación doble",
                Status = "upcoming",
                ParticipantsCount = 64,
                Location = "Arena Gaming · Zona Sur",
                StartAt = baseDate.AddHours(12),
                EndAt = baseDate.AddHours(15),
            },
            new TournamentItem
            {
                Id = 2,
                Name = "Pokémon TCG League",
                Game = "Pokémon TCG",
                Category = "TCG",
                Description = "Competencia de cartas coleccionables formato estándar.",
                Format = "Suizo + Top Cut",
                Status = "upcoming",
                ParticipantsCount = 32,
                Location = "Sala TCG · Zona Central",
                StartAt = baseDate.AddHours(10),
                EndAt = baseDate.AddHours(14),
            },
            new TournamentItem
            {
                Id = 3,
                Name = "Valorant 5v5 Showdown",
                Game = "Valorant",
                Category = "Shooter",
                Description = "Enfrentamientos por equipos con clasificación en vivo.",
                Format = "5v5 · BO3",
                Status = "upcoming",
                ParticipantsCount = 40,
                Location = "Arena eSports · Zona Norte",
                StartAt = baseDate.AddHours(15),
                EndAt = baseDate.AddHours(19),
            },
            new TournamentItem
            {
                Id = 4,
                Name = "League of Legends Clash",
                Game = "League of Legends",
                Category = "MOBA",
                Description = "Torneo por equipos con fase de grupos y playoffs.",
                Format = "5v5 · BO1/BO3",
                Status = "upcoming",
                ParticipantsCount = 40,
                Location = "Arena eSports · Zona Norte",
                StartAt = baseDate.AddDays(1).AddHours(11),
                EndAt = baseDate.AddDays(1).AddHours(17),
            },
            new TournamentItem
            {
                Id = 5,
                Name = "EA Sports FC Cup",
                Game = "EA Sports FC 26",
                Category = "Sports",
                Description = "Competencia 1v1 con premios para los finalistas.",
                Format = "Eliminación simple",
                Status = "upcoming",
                ParticipantsCount = 32,
                Location = "Arena Gaming · Zona Sur",
                StartAt = baseDate.AddDays(1).AddHours(13),
                EndAt = baseDate.AddDays(1).AddHours(16),
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
        var filtered = _allTournaments.Where(tournament =>
        {
            bool matchesCategory =
                SelectedCategory == "Todos" ||
                tournament.Category == SelectedCategory;

            bool matchesSearch =
                string.IsNullOrWhiteSpace(SearchText) ||
                tournament.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                tournament.Game.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                tournament.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                tournament.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                tournament.Format.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                tournament.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        })
        .OrderBy(t => t.StartAt)
        .ToList();

        Tournaments.Clear();

        foreach (var tournament in filtered)
            Tournaments.Add(tournament);

        HasResults = Tournaments.Count > 0;
        HasNoResults = !HasResults;
    }
}