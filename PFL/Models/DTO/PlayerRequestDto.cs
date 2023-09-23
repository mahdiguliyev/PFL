using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.DTO
{
    public class PlayerRequestDto
    {
         public int Id { get; set; }

        public int RequestClubId { get; set; }

        [Required(ErrorMessage = "Oyunçunun adı daxil edilməlidir")]
        [StringLength(100)]
        public string PlayerFirstName { get; set; }

        [StringLength(100)]
        public string PlayerLastName { get; set; }

        [StringLength(100)]
        public string PlayerFatherName { get; set; }

        [Required(ErrorMessage = "Doğum tarixi daxil edilməlidir")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Vətəndaşlıq seçilməlidir")]
        public int CitizenshipId { get; set; }

        [Required(ErrorMessage = "Oyunçunun statusu seçilməlidir")]
        public int ContractTypeId { get; set; }

        public int? FromClubId { get; set; }

        [StringLength(100)]
        public string FromClubName { get; set; }

        [Required(ErrorMessage = "Müqavilənin başlama tarixi daxil edilməlidir")]
        public DateTime ContractBeginDate { get; set; }

        [Required(ErrorMessage = "Müqavilənin bitmə tarixi daxil edilməlidir")]
        public DateTime ContractEndDate { get; set; }

        //[Required(ErrorMessage = "Futbolçunun nömrəsi daxil edilməlidir")]
        //public int? PlayerNumber { get; set; }

        [Required(ErrorMessage = "Futbolçunun mövqeyi seçilməlidir")]
        public int PositionId { get; set; }



        public string PlayerPhotoUrl { get; set; }

        [Required(ErrorMessage = "Futbolçunun şəkli yüklənməyib")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerPhotoUpload { get; set; }




        public string PlayerPasportUrl { get; set; }
        
        [Required(ErrorMessage = "Pasport yüklənməyib")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerPasportUpload { get; set; }
        


        public string PlayerContractUrl { get; set; }

        [Required(ErrorMessage = "Kontrakt yüklənməyib")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerContractUpload { get; set; }



        public string PlayerOtkripleniyaUrl { get; set; }

        //[Required(ErrorMessage = "İTC və ya oyunçu pasportu əlavə et")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerOtkripleniyaUpload { get; set; }


        public string PlayerTmsUrl { get; set; }

        //[Required(ErrorMessage = "TMS yüklənməyib")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerTmsUpload { get; set; }



        public bool ClubConfirm { get; set; }
        

        //public bool OperatorConfirm { get; set; }
    }



    public class PlayerRequestEditDto
    {
        public int Id { get; set; }

        public int RequestClubId { get; set; }

        [Required(ErrorMessage = "Oyunçunun adı daxil edilməlidir")]
        [StringLength(100)]
        public string PlayerFirstName { get; set; }

        [StringLength(100)]
        public string PlayerLastName { get; set; }

        [StringLength(100)]
        public string PlayerFatherName { get; set; }

        public DateTime BirthDate { get; set; }

        public int CitizenshipId { get; set; }

        public int ContractTypeId { get; set; }

        public int? FromClubId { get; set; }

        [StringLength(100)]
        public string FromClubName { get; set; }


        public DateTime ContractBeginDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public int PlayerNumber { get; set; }

        public int PositionId { get; set; }



        public string PlayerPhotoUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerPhotoUpload { get; set; }




        public string PlayerPasportUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerPasportUpload { get; set; }



        public string PlayerContractUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerContractUpload { get; set; }



        public string PlayerOtkripleniyaUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerOtkripleniyaUpload { get; set; }


        public string PlayerTmsUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PlayerTmsUpload { get; set; }



        public bool ClubConfirm { get; set; }


        //public bool OperatorConfirm { get; set; }
    }


    public class PlayerRequestOperatorDto
    {
        public int Id { get; set; }
        public bool OperatorConfirm { get; set; }
        public string RejectionNote { get; set; }
    }
}