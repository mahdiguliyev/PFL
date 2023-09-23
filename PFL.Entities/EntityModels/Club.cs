using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed partial class Club : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Club()
        {
            Players = new HashSet<Player>();
            Matches = new HashSet<Match>();
            Matches1 = new HashSet<Match>();
            TournamentClubs = new HashSet<TournamentClub>();
            TournamentClubs = new HashSet<TournamentClub>();

            ClubUsers = new HashSet<ClubUser>();
            MatchClubPlayers = new HashSet<MatchClubPlayer>();
            MatchResults = new HashSet<MatchResult>();
            MatchPenaltyResults = new HashSet<MatchPenaltyResult>();

            TransfersFrom = new HashSet<Transfer>();
            TransfersTo = new HashSet<Transfer>();

            ClubPlayerOrders = new HashSet<ClubPlayerOrder>();
            ClubOfficials = new HashSet<ClubOfficial>();

            ClubOfficialOrders = new HashSet<ClubOfficialOrder>();
            MatchClubOfficials = new HashSet<MatchClubOfficial>();

            MatchClubPlayerShifts = new HashSet<MatchClubPlayerShift>();

            ClubPlayerRequestsRequest = new HashSet<ClubPlayerRequest>();
            ClubPlayerRequestsFrom = new HashSet<ClubPlayerRequest>();

            //ClubDocumentsOld = new HashSet<ClubDocumentOld>();

            ClubDocuments = new HashSet<ClubDocument>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        [Column("logo_url")]
        [StringLength(1000)]
        public string LogoUrl { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("creation_year")]
        public int CreationYear { get; set; }

        [StringLength(200)]
        public string Stadium { get; set; }

        [Column("stadium_capacity")]
        public int? StadiumCapacity { get; set; }

        [Column("stadium_photo_url")]
        [StringLength(1000)]
        public string StadiumPhotoUrl { get; set; }

        [Column("stadium_document_url")]
        [StringLength(1000)]
        public string StadiumDocumentUrl { get; set; }

        [Column("stadium_scheme_url")]
        [StringLength(1000)]
        public string StadiumSchemeUrl { get; set; }

        [Column("structure_url")]
        [StringLength(1000)]
        public string StructureUrl { get; set; }

        [Column("voen_url")]
        [StringLength(1000)]
        public string VoenUrl { get; set; }

        [Column("charter_url")]
        [StringLength(1000)]
        public string CharterUrl { get; set; }





        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Column("web_site")]
        [StringLength(150)]
        public string WebSite { get; set; }

        [Column("club_president")]
        [StringLength(200)]
        public string ClubPresident { get; set; }

        public int? Status { get; set; }

        public string Note { get; set; }



        public District District { get; set; }

        public User User { get; set; }

        public User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Player> Players { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentClub> TournamentClubs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> Matches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> Matches1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubUser> ClubUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchResult> MatchResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Transfer> TransfersFrom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Transfer> TransfersTo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrder> ClubPlayerOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficial> ClubOfficials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrder> ClubOfficialOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubOfficial> MatchClubOfficials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayerShift> MatchClubPlayerShifts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequestsRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequestsFrom { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public ICollection<ClubDocumentOld> ClubDocumentsOld { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocument> ClubDocuments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchPenaltyResult> MatchPenaltyResults { get; set; }

    }
}
