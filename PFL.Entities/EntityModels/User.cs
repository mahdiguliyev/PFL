using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Clubs = new HashSet<Club>();
            Clubs1 = new HashSet<Club>();
            ContractTypes = new HashSet<ContractType>();
            ContractTypes1 = new HashSet<ContractType>();
            Countries = new HashSet<Country>();
            Countries1 = new HashSet<Country>();
            Districts = new HashSet<District>();
            Districts1 = new HashSet<District>();
            Players = new HashSet<Player>();
            Players1 = new HashSet<Player>();
            Positions = new HashSet<Position>();
            Positions1 = new HashSet<Position>();
            Roles = new HashSet<Role>();
            Tournaments = new HashSet<Tournament>();
            Tournaments1 = new HashSet<Tournament>();
            TournamentTypes = new HashSet<TournamentType>();
            TournamentTypes1 = new HashSet<TournamentType>();
            Users1 = new HashSet<User>();
            Users11 = new HashSet<User>();
            UserTokens = new HashSet<UserToken>();

            TournamentClubs = new HashSet<TournamentClub>();
            TournamentClubs1 = new HashSet<TournamentClub>();

            Matches = new HashSet<Match>();
            Matches1 = new HashSet<Match>();

            ClubUsers = new HashSet<ClubUser>();
            ClubUsers1 = new HashSet<ClubUser>();
            ClubUsers2 = new HashSet<ClubUser>();

            TournamentTours = new HashSet<TournamentTour>();
            TournamentTours1 = new HashSet<TournamentTour>();

            MatchClubPlayers = new HashSet<MatchClubPlayer>();
            MatchClubPlayers1 = new HashSet<MatchClubPlayer>();

            MatchResults = new HashSet<MatchResult>();
            MatchResults1 = new HashSet<MatchResult>();

            MatchPenaltyResultsCreatedBy = new HashSet<MatchPenaltyResult>();
            MatchPenaltyResultsUpdateBy = new HashSet<MatchPenaltyResult>();

            TransfersCreatedBy = new HashSet<Transfer>();
            TransfersLastUpdateBy = new HashSet<Transfer>();

            CardReasonsCreatedBy = new HashSet<CardReason>();
            CardReasonsLastUpdateBy = new HashSet<CardReason>();

            ClubPlayerOrdersCreatedBy = new HashSet<ClubPlayerOrder>();
            ClubPlayerOrdersLastUpdateBy = new HashSet<ClubPlayerOrder>();
            ClubPlayerOrdersClubConfirmBy = new HashSet<ClubPlayerOrder>();
            ClubPlayerOrdersOperatorConfirmBy = new HashSet<ClubPlayerOrder>();

            ClubOfficialsCreatedBy = new HashSet<ClubOfficial>();
            ClubOfficialsLastUpdateBy = new HashSet<ClubOfficial>();
            ClubOfficialsClubConfirmBy = new HashSet<ClubOfficial>();
            ClubOfficialsOperatorConfirmBy = new HashSet<ClubOfficial>();

            ClubOfficialOrdersClubConfirmBy = new HashSet<ClubOfficialOrder>();
            ClubOfficialOrdersOperatorConfirmBy = new HashSet<ClubOfficialOrder>();
            ClubOfficialOrdersCreatedBy = new HashSet<ClubOfficialOrder>();
            ClubOfficialOrdersLastUpdateBy = new HashSet<ClubOfficialOrder>();

            MatchClubOfficialsCreatedBy = new HashSet<MatchClubOfficial>();
            MatchClubOfficialsLastUpdateBy = new HashSet<MatchClubOfficial>();

            MatchClubPlayerShiftsCreatedBy = new HashSet<MatchClubPlayerShift>();
            MatchClubPlayerShiftsLastUpdateBy = new HashSet<MatchClubPlayerShift>();

            PlayerTournamentPenaltiesCreatedBy = new HashSet<PlayerTournamentPenalty>();
            PlayerTournamentPenaltiesLastUpdateBy = new HashSet<PlayerTournamentPenalty>();

            OfficialTournamentPenaltiesCreatedBy = new HashSet<OfficialTournamentPenalty>();
            OfficialTournamentPenaltiesLastUpdateBy = new HashSet<OfficialTournamentPenalty>();

            ClubPlayerRequestsClubConfirmBy = new HashSet<ClubPlayerRequest>();
            ClubPlayerRequestsOperatorConfirmBy = new HashSet<ClubPlayerRequest>();
            ClubPlayerRequestsLastUpdateBy = new HashSet<ClubPlayerRequest>();
            ClubPlayerRequestsCreatedBy = new HashSet<ClubPlayerRequest>();

            ClubPlayerRequestRejectionsCreatedBy = new HashSet<ClubPlayerRequestRejection>();
            ClubPlayerRequestRejectionsLastUpdateBy = new HashSet<ClubPlayerRequestRejection>();

            ClubOfficialRejectionsLastUpdateBy = new HashSet<ClubOfficialRejection>();
            ClubOfficialRejectionsCreatedBy = new HashSet<ClubOfficialRejection>();

            ClubOfficialOrderRejectionsCreatedBy = new HashSet<ClubOfficialOrderRejection>();
            ClubOfficialOrderRejectionsLastUpdateBy = new HashSet<ClubOfficialOrderRejection>();

            ClubPlayerOrderRejectionsCreatedBy = new HashSet<ClubPlayerOrderRejection>();
            ClubPlayerOrderRejectionsLastUpdateBy = new HashSet<ClubPlayerOrderRejection>();

            //ClubDocumentsCreatedBy = new HashSet<ClubDocumentOld>();
            //ClubDocumentsLastUpdateBy = new HashSet<ClubDocumentOld>();

            ClubDocumentNamesCreatedBy = new HashSet<ClubDocumentName>();
            ClubDocumentNamesLastUpdateBy = new HashSet<ClubDocumentName>();
            ClubDocumentRejectionsCreatedBy = new HashSet<ClubDocumentRejection>();
            ClubDocumentRejectionsLastUpdateBy = new HashSet<ClubDocumentRejection>();
            ClubDocumentsCreatedBy = new HashSet<ClubDocument>();
            ClubDocumentsLastUpdateBy = new HashSet<ClubDocument>();
            ClubDocumentTypesCreatedBy = new HashSet<ClubDocumentType>();
            ClubDocumentTypesLastUpdateBy = new HashSet<ClubDocumentType>();

            StadiumsCreatedBy = new HashSet<Stadium>();
            StadiumsLastUpdateBy = new HashSet<Stadium>();

            OffGameTypesCreatedBy = new HashSet<OffGameType>();
            OffGameTypesLastUpdateBy = new HashSet<OffGameType>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("user_name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Password { get; set; }

        [StringLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Column("father_name")]
        public string FatherName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("referee_id")]
        public int? RefereeId { get; set; }

        [Column("last_updated_date")]
        public DateTime? LastUpdatedDate { get; set; }

        [Column("last_update_by_id")]
        public Guid? LastUpdateById { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("created_by_id")]
        public Guid? CreatedById { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Club> Clubs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Club> Clubs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ContractType> ContractTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ContractType> ContractTypes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Country> Countries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Country> Countries1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<District> Districts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<District> Districts1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Player> Players { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Player> Players1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Position> Positions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Position> Positions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Role> Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Tournament> Tournaments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Tournament> Tournaments1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentType> TournamentTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentType> TournamentTypes1 { get; set; }

      

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<User> Users1 { get; set; }

        public User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<User> Users11 { get; set; }

        public User User2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<UserToken> UserTokens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentClub> TournamentClubs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentClub> TournamentClubs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> Matches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> Matches1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubUser> ClubUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubUser> ClubUsers1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubUser> ClubUsers2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentTour> TournamentTours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentTour> TournamentTours1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayers1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchResult> MatchResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchResult> MatchResults1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Transfer> TransfersCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Transfer> TransfersLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<CardReason> CardReasonsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<CardReason> CardReasonsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrder> ClubPlayerOrdersCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrder> ClubPlayerOrdersLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrder> ClubPlayerOrdersClubConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrder> ClubPlayerOrdersOperatorConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficial> ClubOfficialsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficial> ClubOfficialsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficial> ClubOfficialsClubConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficial> ClubOfficialsOperatorConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrder> ClubOfficialOrdersCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrder> ClubOfficialOrdersLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrder> ClubOfficialOrdersClubConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrder> ClubOfficialOrdersOperatorConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubOfficial> MatchClubOfficialsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubOfficial> MatchClubOfficialsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayerShift> MatchClubPlayerShiftsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayerShift> MatchClubPlayerShiftsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<PlayerTournamentPenalty> PlayerTournamentPenaltiesCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<PlayerTournamentPenalty> PlayerTournamentPenaltiesLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OfficialTournamentPenalty> OfficialTournamentPenaltiesCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OfficialTournamentPenalty> OfficialTournamentPenaltiesLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequestsClubConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequestsOperatorConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequestsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequestsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequestRejection> ClubPlayerRequestRejectionsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequestRejection> ClubPlayerRequestRejectionsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialRejection> ClubOfficialRejectionsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialRejection> ClubOfficialRejectionsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrderRejection> ClubOfficialOrderRejectionsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrderRejection> ClubOfficialOrderRejectionsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrderRejection> ClubPlayerOrderRejectionsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrderRejection> ClubPlayerOrderRejectionsLastUpdateBy { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public ICollection<ClubDocumentOld> ClubDocumentsCreatedBy { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public ICollection<ClubDocumentOld> ClubDocumentsLastUpdateBy { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentName> ClubDocumentNamesCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentName> ClubDocumentNamesLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentRejection> ClubDocumentRejectionsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentRejection> ClubDocumentRejectionsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocument> ClubDocumentsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocument> ClubDocumentsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentType> ClubDocumentTypesCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentType> ClubDocumentTypesLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Stadium> StadiumsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Stadium> StadiumsLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OffGameType> OffGameTypesCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OffGameType> OffGameTypesLastUpdateBy { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchPenaltyResult> MatchPenaltyResultsCreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchPenaltyResult> MatchPenaltyResultsUpdateBy { get; set; }
    }
}
