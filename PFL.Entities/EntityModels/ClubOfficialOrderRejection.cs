using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class ClubOfficialOrderRejection : BaseModel
    {
        public int Id { get; set; }

        [Column("club_official_order_id")]
        public int ClubOfficialOrderId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("rejection_note")]
        public string RejectionNote { get; set; }


        public virtual ClubOfficialOrder ClubOfficialOrder { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}