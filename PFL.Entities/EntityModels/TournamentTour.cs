using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class TournamentTour : BaseModel
    {
        public int Id { get; set; }

        [Column("tour_number")]
        public int TourNumber { get; set; }

        [Column("tour_label")]
        public string TourLabel { get; set; }

        [Column("tournament_id")]
        public int TournamentId { get; set; }

        [Column("match_id")]
        public int? MatchId { get; set; }

        public virtual Match Match { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}