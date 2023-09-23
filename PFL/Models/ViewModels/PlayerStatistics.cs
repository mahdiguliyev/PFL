using System;
using System.Collections.Generic;
using PFL.Entities.EntityModels.Views;

namespace PFL.Models.ViewModels
{
    public class PlayerStatisticsViewModel
    {

        public PlayerInfo PlayerInfo { get; set; }

        public List<VPlayerStatistic> PlayerStatistics { get; set; }

    }

    public class PlayerInfo
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Citizenship { get; set; }

        public string CurrentClub { get; set; }

        public int? PlayerNumber { get; set; }

        public string Position { get; set; }

        public string PhotoUrl { get; set; }
    }
}