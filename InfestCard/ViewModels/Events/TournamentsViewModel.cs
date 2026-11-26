using InfestCard.Models.Events;

namespace InfestCard.ViewModels.Events;

public partial class TournamentsViewModel : BaseListViewModel<TournamentItem>
{
    public TournamentsViewModel()
    {
        InitializeCategories("Fighting", "TCG", "Shooter", "MOBA", "Sports");
        LoadDefaultTournaments();
        ApplyFilters();
    }

    private void LoadDefaultTournaments()
    {
        var baseDate = DateTime.Today.AddDays(1);

        AllItems.AddRange(
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

    protected override bool MatchesCategory(TournamentItem item, string category)
        => item.Category == category;

    protected override bool MatchesSearch(TournamentItem item, string search)
        => item.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Game.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Format.Contains(search, StringComparison.OrdinalIgnoreCase) ||
           item.Location.Contains(search, StringComparison.OrdinalIgnoreCase);

    protected override IEnumerable<TournamentItem> Order(IEnumerable<TournamentItem> items)
        => items.OrderBy(tournament => tournament.StartAt);
}