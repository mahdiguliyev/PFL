using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.DTO
{
    public class ClubOfficialDto
    {
        public int Id { get; set; }

        public int ClubId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string FatherName { get; set; }

        public int PositionId { get; set; }
        //public string PositionName { get; set; }

        public DateTime ContractBeginDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? CitizenshipId { get; set; }

        public string Contact { get; set; }

        public string PhotoUrl { get; set; }

        [Required(ErrorMessage = "Şəkil əlavə edilməyib.")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PhotoUpload { get; set; }

        public string PassportUrl { get; set; }

        [Required(ErrorMessage = "Passport sahəsi doldurulmayıb")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PassportUpload { get; set; }


        public string ContractUrl { get; set; }

        [Required(ErrorMessage = "Müqavilə sahəsi doldurulmayıb")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ContractUpload { get; set; }

        
        public string TrainerLicenseUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase TrainerLicenseUpload { get; set; }

        public string DoctorDiplomaUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase DoctorDiplomaUpload { get; set; }

        public bool ClubConfirm { get; set; }

        public Guid? ClubConfirmById { get; set; }

        public DateTime? ClubConfirmDate { get; set; }

        public bool Rejected { get; set; }

        public bool OperatorConfirm { get; set; }

        //public Club Club { get; set; }

        //public OfficialPosition Position { get; set; }

        //public Country Country { get; set; }
    }



    public class ClubOfficialEditDto
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public int PositionId { get; set; }

        public DateTime ContractBeginDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? CitizenshipId { get; set; }

        public string Contact { get; set; }

        public string PassportUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PassportUpload { get; set; }

        public string ContractUrl { get; set; }

        [Required(ErrorMessage = "Passport sahəsi doldurulmayıb")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ContractUpload { get; set; }

        public string TrainerLicenseUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase TrainerLicenseUpload { get; set; }

        public string DoctorDiplomaUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase DoctorDiplomaUpload { get; set; }

        public bool ClubConfirm { get; set; }

        public Guid? ClubConfirmById { get; set; }

        public DateTime? ClubConfirmDate { get; set; }

        public bool Rejected { get; set; }

        public bool OperatorConfirm { get; set; }

        //public Club Club { get; set; }

        //public OfficialPosition Position { get; set; }

        //public Country Country { get; set; }
    }


    public class ClubOfficialOperatorDto
    {
        public int Id { get; set; }
        public bool OperatorConfirm { get; set; }
        public string RejectionNote { get; set; }
    }


    public class ClassOfficialOperatorUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string PositionName { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
    }
}