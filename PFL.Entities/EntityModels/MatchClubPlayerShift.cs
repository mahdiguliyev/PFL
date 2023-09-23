using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class MatchClubPlayerShift : BaseModel
    {
        public int Id { get; set; }

        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("player_id")]
        public long PlayerId { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("player_club_type_id")]
        public int? PlayerClubTypeId { get; set; }

        [Column("player_id_out")]
        public long? PlayerIdOut { get; set; }

        [Column("minute_in")]
        public int? MinuteIn { get; set; }

        [Column("minute_in_plus")]
        public int? MinuteInPlus { get; set; }

        [Column("minute_out")]
        public int? MinuteOut { get; set; }

        [Column("minute_out_plus")]
        public int? MinuteOutPlus { get; set; }


        public virtual Club Club { get; set; }

        public virtual Match Match { get; set; }

        public virtual Player Player { get; set; }

        public virtual Player PlayerOut { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}