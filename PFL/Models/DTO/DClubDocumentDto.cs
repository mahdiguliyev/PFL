using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.DTO
{
    public class DClubDocumentDto
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }


        public string DStadiumContractUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DStadiumContractUpload { get; set; }


        public string DStadiumSchemeUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DStadiumSchemeUpload { get; set; }


        public string DClubStructureUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DClubStructureUpload { get; set; }


        public string DVoenUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DVoenUpload { get; set; }


        public string DCharterUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DCharterUpload { get; set; }


        public string DDinLetterUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DDinLetterUpload { get; set; }


        public string DAmbulanceUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DAmbulanceUpload { get; set; }


        public string DRegisterExtractUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase DRegisterExtractUpload { get; set; }

    }
}