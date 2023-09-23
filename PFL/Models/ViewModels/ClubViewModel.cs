using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using PFL.Entities.EntityModels;


namespace PFL.Models.ViewModels
{
    public class ClubViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        [Column("district_id")]
        public int DistrictId { get; set; }

        [Column("creation_year")]
        public int CreationYear { get; set; }

        [StringLength(200)]
        public string Stadium { get; set; }

        [Column("stadium_capacity")]
        public int? StadiumCapacity { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Column("web_site")]
        [StringLength(150)]
        public string WebSite { get; set; }

        [Column("club_president")]
        [StringLength(200)]
        public string ClubPresident { get; set; }

        //public int? Status { get; set; }

        [AllowHtml]
        public string Note { get; set; }



        public string LogoUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase LogoUpload { get; set; }




        public string StadiumPhotoUrl { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase StadiumPhotoUpload { get; set; }




        public virtual District District { get; set; }
    }
}