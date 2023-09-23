using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed partial class Player : BaseModel
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
            MatchClubPlayers = new HashSet<MatchClubPlayer>();
            //MatchClubPlayersPlayerOut = new HashSet<MatchClubPlayer>();
            MatchResults = new HashSet<MatchResult>();
            MatchPenaltyResults = new HashSet<MatchPenaltyResult>();
            Transfers = new HashSet<Transfer>();
            ClubTournamentPlayers = new HashSet<ClubPlayerOrder>();

            MatchClubPlayerShiftsPlayer = new HashSet<MatchClubPlayerShift>();
            MatchClubPlayerShiftsPlayerOut = new HashSet<MatchClubPlayerShift>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Column("last_name")]
        public string LastName { get; set; }

        [StringLength(100)]
        [Column("father_name")]
        public string FatherName { get; set; }

        [Column("birth_date", TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [Column("citizenship_id")]
        public int CitizenshipId { get; set; }

        [Column("current_club_id")]
        public int? CurrentClubId { get; set; }

        [Column("is_reverse_player")]
        public bool IsReversePlayer { get; set; }

        [Column("contract_type_id")]
        public int? ContractTypeId { get; set; }

        [Column("player_number")]
        public int? PlayerNumber { get; set; }

        [Column("position_type_id")]
        public int PositionTypeId { get; set; }

        [StringLength(1000)]
        [Column("photo_url")]
        public string PhotoUrl { get; set; }

      


        public Club CurrentClub { get; set; }

        public ContractType ContractType { get; set; }

        public Country Country { get; set; }

        public Position Position { get; set; }

        public User User { get; set; }

        public User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayers { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public ICollection<MatchClubPlayer> MatchClubPlayersPlayerOut { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchResult> MatchResults { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchPenaltyResult> MatchPenaltyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Transfer> Transfers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrder> ClubTournamentPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayerShift> MatchClubPlayerShiftsPlayer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayerShift> MatchClubPlayerShiftsPlayerOut { get; set; }
    }
}
