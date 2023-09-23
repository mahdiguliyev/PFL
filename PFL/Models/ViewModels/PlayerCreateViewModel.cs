using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.ViewModels
{
    public class PlayerCreateViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string FatherName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int CitizenshipId { get; set; }

        public int? CurrentClubId { get; set; }
        public string CurrentClubName { get; set; }

        public bool IsReversePlayer { get; set; }

        [StringLength(100)]
        public string FromClubName { get; set; }

        public int? FromClubId { get; set; }

        public DateTime? ContractBeginDate { get; set; }

        public DateTime? ContractEndDate { get; set; }
        
        public int? ContractTypeId { get; set; }

        public int? PlayerNumber { get; set; }

        public int PositionTypeId { get; set; }


        public string PhotoPath { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PhotoUpload { get; set; }
    }
}