using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.DTO
{
    public class AClubDocumentDto
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }


        public string AStadiumContractUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AStadiumContractUpload { get; set; }


        public string AStadiumSchemeUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AStadiumSchemeUpload { get; set; }


        public string AClubStructureUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AClubStructureUpload { get; set; }


        public string AVoenUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AVoenUpload { get; set; }


        public string ACharterUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ACharterUpload { get; set; }


        public string AFhnLetterUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AFhnLetterUpload { get; set; }


        public string ADinLetterUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ADinLetterUpload { get; set; }

        public string AAmbulanceUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AAmbulanceUpload { get; set; }


        public string ARegisterExtractUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ARegisterExtractUpload { get; set; }


        public string AAffaLicenseUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AAffaLicenseUpload { get; set; }

    }
}