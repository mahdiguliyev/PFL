using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_players")]
    public class VPlayer
    {
        [Key]
        public long Id { get; set; }
        public int? PlayerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Citizenship { get; set; }
        public string PhotoUrl { get; set; }
        public string PositionName { get; set; }
        public string PreviousClub { get; set; }
        public string CurrentClub { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string ContractTypeName { get; set; }
        public bool IsDeleted { get; set; }
    }
}