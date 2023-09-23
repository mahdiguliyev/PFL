using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class MatchClubPlayer : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MatchClubPlayer()
        {
            PlayerTournamentPenalties = new HashSet<PlayerTournamentPenalty>();
        }
        public int Id { get; set; }
        
        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("player_id")]
        public long PlayerId { get; set; }

        [Column("player_number")]
        public int? PlayerNumber { get; set; }

        [Column("player_club_type_id")]
        public int? PlayerClubTypeId { get; set; }

        //[Column("player_id_out")]
        //public long? PlayerIdOut { get; set; }

        //[Column("minute_in")]
        //public int? MinuteIn { get; set; }

        //[Column("minute_inp")]
        //public int? MinuteInp { get; set; }

        //[Column("minute_out")]
        //public int? MinuteOut { get; set; }

        //[Column("minute_outp")]
        //public int? MinuteOutp { get; set; }

        [Column("yellow_minute")]
        public int? YellowMinute { get; set; }

        [Column("yellow_minute_plus")]
        public int? YellowMinutePlus { get; set; }

        [Column("yellow_offgame_type_id")]
        public int? YellowOffgameTypeId { get; set; }

        [Column("yellow_reason_id")]
        public int? YellowReasonId { get; set; }

        [Column("yellow2_minute")]
        public int? Yellow2Minute { get; set; }

        [Column("yellow2_minute_plus")]
        public int? Yellow2MinutePlus { get; set; }

        [Column("yellow2_offgame_type_id")]
        public int? Yellow2OffgameTypeId { get; set; }

        [Column("yellow2_reason_id")]
        public int? Yellow2ReasonId { get; set; }

        [Column("red_minute")]
        public int? RedMinute { get; set; }

        [Column("red_minute_plus")]
        public int? RedMinutePlus { get; set; }

        [Column("red_offgame_type_id")]
        public int? RedOffgameTypeId { get; set; }

        [Column("red_reason_id")]
        public int? RedReasonId { get; set; }

        public int? Num { get; set; }

        public int? Captain { get; set; }

        public int? Played { get; set; }


        public Club Club { get; set; }

        
        public Player Player { get; set; }

        //public virtual Player PlayerOut { get; set; }

        public Match Match { get; set; }

        public OffGameType OffGameTypeYellow { get; set; }

        public OffGameType OffGameTypeYellow2 { get; set; }

        public OffGameType OffGameTypeRed { get; set; }

        public CardReason CardReasonYellow1 { get; set; }
               
        public CardReason CardReasonYellow2 { get; set; }
               
        public CardReason CardReasonRed { get; set; }


        public User UserLastUpdateBy { get; set; }

        public User UserCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<PlayerTournamentPenalty> PlayerTournamentPenalties { get; set; }

    }
}