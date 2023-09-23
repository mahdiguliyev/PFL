using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_tournament_player_table")]
    public class VTournamentPlayerTable
    {
        //this id is just for key
        //public Guid Id { get; set; }

        [Key]
        public long PlayerId { get; set; }

        public int TournamentId { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        //public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int PlayerGoalCount { get; set; }
    }
}