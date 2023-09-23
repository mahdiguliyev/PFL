using System;
using System.Collections.Generic;
using PFL.Models.DTO;

namespace PFL.Models.ViewModels
{
    public class MatchDocumentViewModel
    {
        public MatchInfo MatchInfo { get; set; }
        public MatchOfficials MatchOfficials { get; set; }
        public MatchScoreInfo MatchScoreInfo { get; set; }
        public List<MatchGoal> MatchGoals { get; set; }
        public List<MatchPenaltyGoal> MatchPenaltyGoals { get; set; }
        public List<MatchPlayerShiftDto> MatchPlayerShifts { get; set; }

        public List<MatchPenaltyCardDto> MatchPenaltyCards { get; set; }
    }

    public class MatchInfo
    {
        public int SeasonId { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public int TournamentTypeId { get; set; }

        public int TournamentSeasonStartYear { get; set; }
        public int TournamentSeasonEndYear { get; set; }

        public int? HomeClubId { get; set; }
        public string HomeClubName { get; set; }
        public int? GuestClubId { get; set; }
        public string GuestClubName { get; set; }
        public int TourNumber { get; set; }
        public string TourLabel { get; set; }

        public bool HomeClubTechnicalDefeat { get; set; }
        public string HomeClubTechnicalDefeatNote { get; set; }
        public bool GuestClubTechnicalDefeat { get; set; }
        public string GuestClubTechnicalDefeatNote { get; set; }

        public DateTime? MatchDate { get; set; }
        public string Place { get; set; }
        public string Stadium { get; set; }
        public int? StadiumAudienceCount { get; set; }

        public int? HalfTimeExtra { get; set; }
        public int? FullTimeExtra { get; set; }

        public int? Half2TimeExtra { get; set; }
        public int? Full2TimeExtra { get; set; }

        public string CupNote { get; set; }
        public string RefereeNote { get; set; }
        public string HomeClubNote { get; set; }
        public string GuestClubNote { get; set; }

        public int YoungPlayerAge21Limit { get; set; }
        public int YoungPlayerAge19Limit { get; set; }
    }

    public class MatchOfficials
    {
        public MatchOfficial Referee { get; set; }
        public MatchOfficial RefereeAssistant1 { get; set; }
        public MatchOfficial RefereeAssistant2 { get; set; }
        public MatchOfficial FourthReferee { get; set; }
        public MatchOfficial AdditionalReferee1 { get; set; }
        public MatchOfficial AdditionalReferee2 { get; set; }

        public MatchOfficial RefereeVar { get; set; }
        public MatchOfficial RefereeAvar { get; set; }

        public MatchOfficial AffaRepresentative { get; set; }
        public MatchOfficial RefereeInspector { get; set; }
    }

    public class MatchOfficial
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }
        public string City { get; set; }
    }

    public class MatchScoreInfo
    {
        public MatchScore HalfTimeScore { get; set; }
        public MatchScore FullTimeScore { get; set; }
    }

    public class MatchScore
    {
        public int HomeClubScore { get; set; }
        public int GuestClubScore { get; set; }
    }

    public class MatchGoal
    {
        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public long? PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public int? Minute { get; set; }
        public int? MinutePlus { get; set; }
        public int? GoalTypeId { get; set; }
        public string GoalType { get; set; }
    }



    public class MatchPenaltyGoal
    {
        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public long? PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public int PenaltyOrder { get; set; }
        //public int? MinutePlus { get; set; }
        public int? GoalTypeId { get; set; }
        public string GoalType { get; set; }
    }
}