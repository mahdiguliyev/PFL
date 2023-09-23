using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed partial class Match : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Match()
        {
            MatchClubPlayers = new HashSet<MatchClubPlayer>();
            TournamentTours = new HashSet<TournamentTour>();
            MatchResults = new HashSet<MatchResult>();
            MatchPenaltyResults = new HashSet<MatchPenaltyResult>();
            MatchClubOfficials = new HashSet<MatchClubOfficial>();
            MatchClubPlayerShifts = new HashSet<MatchClubPlayerShift>();
        }


        public int Id { get; set; }

        [Column("home_club_id")]
        public int? HomeClubId { get; set; }

        [Column("guest_club_id")]
        public int? GuestClubId { get; set; }


        [Column("home_club_technical_defeat")]
        public bool HomeClubTechnicalDefeat { get; set; }

        [Column("home_club_technical_defeat_note")]
        public string HomeClubTechnicalDefeatNote { get; set; }

        [Column("guest_club_technical_defeat")]
        public bool GuestClubTechnicalDefeat { get; set; }

        [Column("guest_club_technical_defeat_note")]
        public string GuestClubTechnicalDefeatNote { get; set; }



        //public string Stadium { get; set; }

        [Column("stadium_id")]
        public int? StadiumId { get; set; }

        [Column("audience_count")]
        public int? AudienceCount { get; set; }

        [Column("home_club_score")]
        public int? HomeClubScore { get; set; }

        [Column("guest_club_score")]
        public int? GuestClubScore { get; set; }

        [Column("match_date")]
        public DateTime? MatchDate { get; set; }

        [Column("home_club_first_extra_score")]
        public int? HomeClubFirstExtraScore { get; set; }

        [Column("half_extra_time")]
        public int? HalfExtraTime { get; set; }

        [Column("full_extra_time")]
        public int? FullExtraTime { get; set; }

        [Column("half2_extra_time")]
        public int? Half2ExtraTime { get; set; }

        [Column("full2_extra_time")]
        public int? Full2ExtraTime { get; set; }


        [Column("home_club_second_extra_score")]
        public int? HomeClubSecondExtraScore { get; set; }

        [Column("guest_club_first_extra_score")]
        public int? GuestClubFirstExtraScore { get; set; }

        [Column("guest_club_second_extra_score")]
        public int? GuestClubSecondExtraScore { get; set; }

        [Column("home_club_penalty_score")]
        public int? HomeClubPenaltyScore { get; set; }

        [Column("guest_club_penalty_score")]
        public int? GuestClubPenaltyScore { get; set; }

        public int? Winner { get; set; }

        [Column("referee_id")]
        public int? RefereeId { get; set; }

        [Column("referee_assistant1_id")]
        public int? RefereeAssistant1Id { get; set; }

        [Column("referee_assistant2_id")]
        public int? RefereeAssistant2Id { get; set; }

        [Column("fourth_referee_id")]
        public int? FourthRefereeId { get; set; }

        [Column("additional_referee1_id")]
        public int? AdditionalReferee1Id { get; set; }

        [Column("additional_referee2_id")]
        public int? AdditionalReferee2Id { get; set; }

        [Column("affa_representative_id")]
        public int? AffaRepresentativeId { get; set; }

        [Column("referee_inspector_id")]
        public int? RefereeInspectorId { get; set; }

        [Column("referee_var_id")]
        public int? RefereeVarId { get; set; }

        [Column("referee_avar_id")]
        public int? RefereeAvarId { get; set; }

        [Column("cup_note")]
        public string CupNote { get; set; }

        [Column("referee_note")]
        public string RefereeNote { get; set; }

        [Column("home_club_note")]
        public string HomeClubNote { get; set; }

        [Column("guest_club_note")]
        public string GuestClubNote { get; set; }


        [Column("referee_confirm")]
        public bool RefereeConfirm { get; set; }

        [Column("referee_confirmed_date")]
        public DateTime? RefereeConfirmedDate { get; set; }

        [Column("home_club_confirm")]
        public bool? HomeClubConfirm { get; set; }

        [Column("home_club_confirmed_date")]
        public DateTime? HomeClubConfirmedDate { get; set; }

        [Column("home_club_exp_confirm_allow")]
        public bool HomeClubExpConfirmAllow { get; set; }

        [Column("guest_club_confirm")]
        public bool? GuestClubConfirm { get; set; }

        [Column("guest_club_confirmed_date")]
        public DateTime? GuestClubConfirmedDate { get; set; }

        [Column("guest_club_exp_confirm_allow")]
        public bool GuestClubExpConfirmAllow { get; set; }

        public Club HomeClub { get; set; }

        public Club GuestClub { get; set; }

        public User UserLastUpdateBy { get; set; }

        public User UserCreatedBy { get; set; }
        public Referee Referee { get; set; }

        public Referee RefereeAssistant1 { get; set; }

        public Referee RefereeAssistant2 { get; set; }

        public Referee FourthReferee { get; set; }

        public Referee AdditionalReferee1 { get; set; }

        public Referee AdditionalReferee2 { get; set; }
        public Referee RefereeVar { get; set; }
        public Referee RefereeAvar { get; set; }

        public Referee AffaRepresentative { get; set; }

        public Referee RefereeInspector { get; set; }

        public Stadium Stadium { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentTour> TournamentTours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchResult> MatchResults { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchPenaltyResult> MatchPenaltyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubOfficial> MatchClubOfficials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayerShift> MatchClubPlayerShifts { get; set; }

    }
}