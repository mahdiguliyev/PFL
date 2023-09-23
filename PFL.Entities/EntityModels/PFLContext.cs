using PFL.Entities.EntityModels.Views;
using System.Data.Entity;

namespace PFL.Entities.EntityModels
{
    public partial class PFLContext : DbContext
    {
        public PFLContext()
            : base("name=PFLModels")
        {
        }

        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<ContractType> ContractTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }
        public virtual DbSet<TournamentType> TournamentTypes { get; set; }
        //public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<TournamentClub> TournamentClubs { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<ClubUser> ClubUsers { get; set; }
        public virtual DbSet<TournamentTour> TournamentTours { get; set; }

        public virtual DbSet<MatchClubPlayer> MatchClubPlayers { get; set; }
        public virtual DbSet<MatchResult> MatchResults { get; set; }
        public virtual DbSet<MatchPenaltyResult> MatchPenaltyResults { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }
        //public virtual DbSet<VTournamentTable> VTournamentTables { get; set; }
        public virtual DbSet<VTournamentPlayerTable> VTournamentPlayerTables { get; set; }
        //public virtual DbSet<VTournamentCardStatistic> VTournamentCardStatistics { get; set; }
        public virtual DbSet<Referee> Referees { get; set; }
        public virtual DbSet<GoalType> GoalTypes { get; set; }
        public virtual DbSet<CardReason> CardReasons { get; set; }
        public virtual DbSet<ClubPlayerOrder> ClubPlayerOrders { get; set; }
        public virtual DbSet<ClubOfficial> ClubOfficials { get; set; }
        public virtual DbSet<ClubOfficialRejection> ClubOfficialRejections { get; set; }
        public virtual DbSet<OfficialPosition> OfficialPositions { get; set; }
        public virtual DbSet<ClubOfficialOrder> ClubOfficialOrders { get; set; }
        public virtual DbSet<ClubType> ClubTypes { get; set; }
        public virtual DbSet<MatchClubOfficial> MatchClubOfficials { get; set; }
        public virtual DbSet<RefereeType> RefereeTypes { get; set; }
        public virtual DbSet<MatchClubPlayerShift> MatchClubPlayerShifts { get; set; }
        public virtual DbSet<PlayerTournamentPenalty> PlayerTournamentPenalties { get; set; }
        public virtual DbSet<OfficialTournamentPenalty> OfficialTournamentPenalties { get; set; }
        public virtual DbSet<PenaltyCardType> PenaltyCardTypes { get; set; }
        public virtual DbSet<VPlayerPenalty> VPlayerPenalties { get; set; }
        public virtual DbSet<VOfficialPenalty> VOfficialPenalties { get; set; }
        public virtual DbSet<ClubPlayerRequest> ClubPlayerRequests { get; set; }
        public virtual DbSet<ClubPlayerRequestRejection> ClubPlayerRequestRejections { get; set; }
        //public virtual DbSet<VPlayerStatistic> VPlayerStatistics { get; set; }
        public virtual DbSet<VClub> VClubs { get; set; }
        public virtual DbSet<VClubNotification> VClubNotifications { get; set; }
        public virtual DbSet<VPlayer> VPlayers { get; set; }
        public virtual DbSet<ClubOfficialOrderRejection> ClubOfficialOrderRejections { get; set; }
        public virtual DbSet<ClubPlayerOrderRejection> ClubPlayerOrderRejections { get; set; }
        public virtual DbSet<VOrderNotification> VOrderNotifications { get; set; }
        public virtual DbSet<VClubOrderNotification> VClubOrderNotifications { get; set; }
        //public virtual DbSet<ClubDocumentOld> ClubDocumentsOld { get; set; }
        public virtual DbSet<ClubTournamentType> ClubTournamentTypes { get; set; }
        public virtual DbSet<Stadium> Stadiums { get; set; }


        public virtual DbSet<ClubDocumentName> ClubDocumentNames { get; set; }
        public virtual DbSet<ClubDocumentRejection> ClubDocumentRejections { get; set; }
        public virtual DbSet<ClubDocument> ClubDocuments { get; set; }
        public virtual DbSet<ClubDocumentType> ClubDocumentTypes { get; set; }
        public virtual DbSet<VUsersCount> VUsersCounts { get; set; }

        public virtual DbSet<OffGameType> OffGameTypes { get; set; }
        public virtual DbSet<VProtocolPlayerCard> VProtocolPlayerCards { get; set; }
        public virtual DbSet<VProtocolPlayerShift> VProtocolPlayerShifts { get; set; }
        public virtual DbSet<VProtocolMatchResult> VProtocolMatchResults { get; set; }
        public virtual DbSet<VProtocolMatchPenaltyResult> VProtocolMatchPenaltyResults { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }

        public virtual DbSet<MatchResultLog> MatchResultLogs { get; set; }


        #region LogTables

        public virtual DbSet<LogClubOfficial> LogClubOfficials { get; set; }

        #endregion




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .Map(m => m.ToTable("UserRoles").MapLeftKey("user_id").MapRightKey("role_id"));


            modelBuilder.Entity<Club>()
                .HasMany(e => e.Players)
                .WithOptional(e => e.CurrentClub)
                .HasForeignKey(e => e.CurrentClubId);
            //.WillCascadeOnDelete(false);

            //modelBuilder.Entity<Club>()
            //    .HasMany(e => e.Players1)
            //    .WithOptional(e => e.FromClub)
            //    .HasForeignKey(e => e.FromClubId);

            modelBuilder.Entity<ContractType>()
                .HasMany(e => e.Players)
                .WithOptional(e => e.ContractType)
                .HasForeignKey(e => e.ContractTypeId);
            //.WillCascadeOnDelete(false);

            modelBuilder.Entity<RefereeType>()
                .HasMany(e => e.Referees)
                .WithRequired(e => e.RefereeType)
                .HasForeignKey(e => e.RefereeTypeId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Players)
                .WithRequired(e => e.Country)
                .HasForeignKey(e => e.CitizenshipId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Clubs)
                .WithOptional(e => e.District)
                .HasForeignKey(e => e.DistrictId);
            //.WillCascadeOnDelete(false);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Referees)
                .WithOptional(e => e.District)
                .HasForeignKey(e => e.DistrictId);

            //modelBuilder.Entity<Player>()
            //    .Property(e => e.IsReversePlayer)
            //    .IsFixedLength();

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Players)
                .WithRequired(e => e.Position)
                .HasForeignKey(e => e.PositionTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TournamentType>()
                .HasMany(e => e.Tournaments)
                .WithRequired(e => e.TournamentType)
                .HasForeignKey(e => e.TournamentTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Clubs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Clubs1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ContractTypes)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ContractTypes1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Countries)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Countries1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Districts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Districts1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Players)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Players1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Positions)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Positions1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tournaments)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tournaments1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TournamentTypes)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TournamentTypes1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.CreatedById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users11)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserTokens)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.TournamentClubs)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tournament>()
                .HasMany(e => e.TournamentClubs)
                .WithRequired(e => e.Tournament)
                .HasForeignKey(e => e.TournamentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TournamentClubs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TournamentClubs1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Matches)
                .WithOptional(e => e.HomeClub)
                .HasForeignKey(e => e.HomeClubId);
            //.WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Matches1)
                .WithOptional(e => e.GuestClub)
                .HasForeignKey(e => e.GuestClubId);
            //.WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Matches)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Matches1)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubUsers)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubUsers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubUsers1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubUsers2)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<Match>()
                .HasMany(e => e.TournamentTours)
                .WithOptional(e => e.Match)
                .HasForeignKey(e => e.MatchId);

            modelBuilder.Entity<Tournament>()
                .HasMany(e => e.TournamentTours)
                .WithRequired(e => e.Tournament)
                .HasForeignKey(e => e.TournamentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TournamentTours)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TournamentTours1)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.MatchClubPlayers)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                .HasMany(e => e.MatchClubPlayers)
                .WithRequired(e => e.Match)
                .HasForeignKey(e => e.MatchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.MatchClubPlayers)
                .WithRequired(e => e.Player)
                .HasForeignKey(e => e.PlayerId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Player>()
            //    .HasMany(e => e.MatchClubPlayersPlayerOut)
            //    .WithOptional(e => e.PlayerOut)
            //    .HasForeignKey(e => e.PlayerIdOut);
            ////.WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchClubPlayers)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchClubPlayers1)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.MatchResults)
                .WithOptional(e => e.Club)
                .HasForeignKey(e => e.ClubId);

            modelBuilder.Entity<Match>()
                .HasMany(e => e.MatchResults)
                .WithRequired(e => e.Match)
                .HasForeignKey(e => e.MatchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.MatchResults)
                .WithOptional(e => e.Player)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchResults)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchResults1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<ContractType>()
                .HasMany(e => e.Transfers)
                .WithOptional(e => e.ContractType)
                .HasForeignKey(e => e.ContractTypeId);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.TransfersFrom)
                .WithOptional(e => e.ClubFrom)
                .HasForeignKey(e => e.ClubFromId);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.TransfersTo)
                .WithOptional(e => e.ClubTo)
                .HasForeignKey(e => e.ClubToId);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.Transfers)
                .WithOptional(e => e.Player)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TransfersCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TransfersLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesReferee)
                .WithOptional(e => e.Referee)
                .HasForeignKey(e => e.RefereeId);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesRefereeAssistant1)
                .WithOptional(e => e.RefereeAssistant1)
                .HasForeignKey(e => e.RefereeAssistant1Id);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesRefereeAssistant2)
                .WithOptional(e => e.RefereeAssistant2)
                .HasForeignKey(e => e.RefereeAssistant2Id);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesFourthReferee)
                .WithOptional(e => e.FourthReferee)
                .HasForeignKey(e => e.FourthRefereeId);


            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesRefereeVar)
                .WithOptional(e => e.RefereeVar)
                .HasForeignKey(e => e.RefereeVarId);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesRefereeAvar)
                .WithOptional(e => e.RefereeAvar)
                .HasForeignKey(e => e.RefereeAvarId);






            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesAdditionalReferee1)
                .WithOptional(e => e.AdditionalReferee1)
                .HasForeignKey(e => e.AdditionalReferee1Id);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesAdditionalReferee2)
                .WithOptional(e => e.AdditionalReferee2)
                .HasForeignKey(e => e.AdditionalReferee2Id);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesAffaRepresentative)
                .WithOptional(e => e.AffaRepresentative)
                .HasForeignKey(e => e.AffaRepresentativeId);

            modelBuilder.Entity<Referee>()
                .HasMany(e => e.MatchesRefereeInspector)
                .WithOptional(e => e.RefereeInspector)
                .HasForeignKey(e => e.RefereeInspectorId);

            modelBuilder.Entity<GoalType>()
                .HasMany(e => e.MatchResults)
                .WithOptional(e => e.GoalType)
                .HasForeignKey(e => e.GoalTypeId);

            modelBuilder.Entity<CardReason>()
                .HasMany(e => e.MatchClubPlayersYellow1)
                .WithOptional(e => e.CardReasonYellow1)
                .HasForeignKey(e => e.YellowReasonId);

            modelBuilder.Entity<CardReason>()
                .HasMany(e => e.MatchClubPlayersYellow2)
                .WithOptional(e => e.CardReasonYellow2)
                .HasForeignKey(e => e.Yellow2ReasonId);

            modelBuilder.Entity<CardReason>()
                .HasMany(e => e.MatchClubPlayersRed)
                .WithOptional(e => e.CardReasonRed)
                .HasForeignKey(e => e.RedReasonId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CardReasonsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CardReasonsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdatedBy)
                .HasForeignKey(e => e.LastUpdateById);



            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubPlayerOrders)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.ClubTournamentPlayers)
                .WithRequired(e => e.Player)
                .HasForeignKey(e => e.PlayerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClubType>()
                .HasMany(e => e.ClubPlayerOrders)
                .WithRequired(e => e.ClubType)
                .HasForeignKey(e => e.ClubTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerOrdersCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerOrdersLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerOrdersClubConfirmBy)
                .WithRequired(e => e.UserClubConfirmBy)
                .HasForeignKey(e => e.ClubConfirmById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerOrdersOperatorConfirmBy)
                .WithOptional(e => e.UserOperatorConfirmBy)
                .HasForeignKey(e => e.OperatorConfirmById);

            modelBuilder.Entity<ClubOfficial>()
                .HasMany(e => e.ClubOfficialOrders)
                .WithRequired(e => e.ClubOfficial)
                .HasForeignKey(e => e.ClubOfficialId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubOfficialOrders)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClubType>()
                .HasMany(e => e.ClubOfficialOrders)
                .WithRequired(e => e.ClubType)
                .HasForeignKey(e => e.ClubTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialOrdersClubConfirmBy)
                .WithRequired(e => e.UserClubConfirmBy)
                .HasForeignKey(e => e.ClubConfirmById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialOrdersOperatorConfirmBy)
                .WithOptional(e => e.UserOperatorConfirmBy)
                .HasForeignKey(e => e.OperatorConfirmById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialOrdersCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialOrdersLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<ClubOfficial>()
                .HasMany(e => e.MatchClubOfficials)
                .WithRequired(e => e.ClubOfficial)
                .HasForeignKey(e => e.ClubOfficialId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.MatchClubOfficials)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClubType>()
                .HasMany(e => e.MatchClubOfficials)
                .WithRequired(e => e.ClubType)
                .HasForeignKey(e => e.ClubTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                .HasMany(e => e.MatchClubOfficials)
                .WithRequired(e => e.Match)
                .HasForeignKey(e => e.MatchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchClubOfficialsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchClubOfficialsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<Club>()
                .HasMany(e => e.MatchClubPlayerShifts)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                .HasMany(e => e.MatchClubPlayerShifts)
                .WithRequired(e => e.Match)
                .HasForeignKey(e => e.MatchId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.MatchClubPlayerShiftsPlayer)
                .WithRequired(e => e.Player)
                .HasForeignKey(e => e.PlayerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.MatchClubPlayerShiftsPlayerOut)
                .WithOptional(e => e.PlayerOut)
                .HasForeignKey(e => e.PlayerIdOut);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchClubPlayerShiftsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchClubPlayerShiftsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<MatchClubPlayer>()
                .HasMany(e => e.PlayerTournamentPenalties)
                .WithRequired(e => e.MatchClubPlayer)
                .HasForeignKey(e => e.MatchClubPlayerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PenaltyCardType>()
                .HasMany(e => e.PlayerTournamentPenalties)
                .WithRequired(e => e.PenaltyCardType)
                .HasForeignKey(e => e.PenaltyCardTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PlayerTournamentPenaltiesCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PlayerTournamentPenaltiesLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<MatchClubOfficial>()
                .HasMany(e => e.OfficialTournamentPenalties)
                .WithRequired(e => e.MatchClubOfficial)
                .HasForeignKey(e => e.MatchClubOfficialId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OfficialTournamentPenaltiesCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OfficialTournamentPenaltiesLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubPlayerRequestsFrom)
                .WithOptional(e => e.FromClub)
                .HasForeignKey(e => e.FromClubId);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubPlayerRequestsRequest)
                .WithRequired(e => e.RequestClub)
                .HasForeignKey(e => e.RequestClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerRequestsClubConfirmBy)
                .WithOptional(e => e.UserClubConfirmBy)
                .HasForeignKey(e => e.ClubConfirmById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerRequestsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerRequestsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerRequestsOperatorConfirmBy)
                .WithOptional(e => e.UserOperatorConfirmBy)
                .HasForeignKey(e => e.OperatorConfirmById);


            modelBuilder.Entity<ClubPlayerRequest>()
                .HasMany(e => e.ClubPlayerRequestRejections)
                .WithRequired(e => e.ClubPlayerRequest)
                .HasForeignKey(e => e.ClubPlayerRequestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerRequestRejectionsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerRequestRejectionsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<ContractType>()
                .HasMany(e => e.ClubPlayerRequests)
                .WithRequired(e => e.ContractType)
                .HasForeignKey(e => e.ContractTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.ClubPlayerRequests)
                .WithRequired(e => e.Country)
                .HasForeignKey(e => e.CitizenshipId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.ClubPlayerRequests)
                .WithRequired(e => e.Position)
                .HasForeignKey(e => e.PositionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubOfficials)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.ClubOfficials)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.CitizenshipId);

            modelBuilder.Entity<OfficialPosition>()
                .HasMany(e => e.ClubOfficials)
                .WithRequired(e => e.Position)
                .HasForeignKey(e => e.PositionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialsClubConfirmBy)
                .WithOptional(e => e.UserClubConfirmBy)
                .HasForeignKey(e => e.ClubConfirmById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialsOperatorConfirmBy)
                .WithOptional(e => e.UserOperatorConfirmBy)
                .HasForeignKey(e => e.OperatorConfirmById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClubOfficial>()
                .HasMany(e => e.ClubOfficialRejections)
                .WithRequired(e => e.ClubOfficial)
                .HasForeignKey(e => e.ClubOfficialId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialRejectionsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdate)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialRejectionsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<ClubOfficialOrder>()
                .HasMany(e => e.ClubOfficialOrderRejections)
                .WithRequired(e => e.ClubOfficialOrder)
                .HasForeignKey(e => e.ClubOfficialOrderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialOrderRejectionsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubOfficialOrderRejectionsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<ClubPlayerOrder>()
                .HasMany(e => e.ClubPlayerOrderRejections)
                .WithRequired(e => e.ClubPlayerOrder)
                .HasForeignKey(e => e.ClubPlayerOrderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerOrderRejectionsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubPlayerOrderRejectionsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);



            ////ClubDocumentsOld
            //modelBuilder.Entity<Club>()
            //    .HasMany(e => e.ClubDocumentsOld)
            //    .WithRequired(e => e.Club)
            //    .HasForeignKey(e => e.ClubId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.ClubDocumentsCreatedBy)
            //    .WithRequired(e => e.UserCreatedBy)
            //    .HasForeignKey(e => e.CreatedById)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.ClubDocumentsLastUpdateBy)
            //    .WithOptional(e => e.UserLastUpdateBy)
            //    .HasForeignKey(e => e.LastUpdateById);






            modelBuilder.Entity<ClubDocumentType>()
                .HasMany(e => e.ClubDocumentNames)
                .WithRequired(e => e.ClubDocumentType)
                .HasForeignKey(e => e.ClubDocumentTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentNamesCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentNamesLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<ClubDocument>()
                .HasMany(e => e.ClubDocumentRejections)
                .WithRequired(e => e.ClubDocument)
                .HasForeignKey(e => e.ClubDocumentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentRejectionsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentRejectionsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<ClubDocumentName>()
                .HasMany(e => e.ClubDocuments)
                .WithRequired(e => e.ClubDocumentName)
                .HasForeignKey(e => e.ClubDocumenNameId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubDocuments)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentTypesCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClubDocumentTypesLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<Stadium>()
                .HasMany(e => e.Matches)
                .WithOptional(e => e.Stadium)
                .HasForeignKey(e => e.StadiumId);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Stadiums)
                .WithOptional(e => e.District)
                .HasForeignKey(e => e.DistrictId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.StadiumsCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.StadiumsLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<OffGameType>()
                .HasMany(e => e.MatchClubPlayersYellow)
                .WithOptional(e => e.OffGameTypeYellow)
                .HasForeignKey(e => e.YellowOffgameTypeId);

            modelBuilder.Entity<OffGameType>()
                .HasMany(e => e.MatchClubPlayersYellow2)
                .WithOptional(e => e.OffGameTypeYellow2)
                .HasForeignKey(e => e.Yellow2OffgameTypeId);

            modelBuilder.Entity<OffGameType>()
                .HasMany(e => e.MatchClubPlayersRed)
                .WithOptional(e => e.OffGameTypeRed)
                .HasForeignKey(e => e.RedOffgameTypeId);



            modelBuilder.Entity<User>()
                .HasMany(e => e.OffGameTypesCreatedBy)
                .WithRequired(e => e.UserCreatedBy)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OffGameTypesLastUpdateBy)
                .WithOptional(e => e.UserLastUpdateBy)
                .HasForeignKey(e => e.LastUpdateById);


            modelBuilder.Entity<Club>()
                .HasMany(e => e.MatchPenaltyResults)
                .WithOptional(e => e.Club)
                .HasForeignKey(e => e.ClubId);


            modelBuilder.Entity<GoalType>()
                .HasMany(e => e.MatchPenaltyResults)
                .WithOptional(e => e.GoalType)
                .HasForeignKey(e => e.GoalTypeId);


            modelBuilder.Entity<Match>()
                .HasMany(e => e.MatchPenaltyResults)
                .WithRequired(e => e.Match)
                .HasForeignKey(e => e.MatchId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Player>()
                .HasMany(e => e.MatchPenaltyResults)
                .WithOptional(e => e.Player)
                .HasForeignKey(e => e.PlayerId);


            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchPenaltyResultsCreatedBy)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedById)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.MatchPenaltyResultsUpdateBy)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.LastUpdateById);

        }


    }
}
