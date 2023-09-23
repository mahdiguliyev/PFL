using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.DTO
{
    public class BClubDocumentDto
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }


        public string BStadiumContractUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase BStadiumContractUpload { get; set; }


        public string BStadiumSchemeUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase BStadiumSchemeUpload { get; set; }


        public string BClubStructureUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase BClubStructureUpload { get; set; }


        public string BDinLetterUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase BDinLetterUpload { get; set; }


        public string BAmbulanceUrl { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase BAmbulanceUpload { get; set; }


        //public string BAffaLicenseUrl { get; set; }
        //[DataType(DataType.Upload)]
        //public HttpPostedFileBase BAffaLicenseUpload { get; set; }

    }
}