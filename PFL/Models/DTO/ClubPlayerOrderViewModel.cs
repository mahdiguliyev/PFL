using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PFL.Models.DTO
{
    public class ClubPlayerOrderDto
    {
        public int Id { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        public int ClubId { get; set; }
        public long PlayerId { get; set; }
        public int PlayerNumber { get; set; }
        public int ClubTypeId { get; set; }
        public bool ClubConfirm { get; set; }


        public string HealthFile { get; set; }

        [Required(ErrorMessage = "Sağlamlıq arayışı")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase HealthFileUpload { get; set; }
    }
}