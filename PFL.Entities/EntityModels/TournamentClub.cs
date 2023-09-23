using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{

    public partial class TournamentClub : BaseModel
    {
        public int Id { get; set; }


        [Column("tournament_id")]
        public int TournamentId { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

      

        public virtual Club Club { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}