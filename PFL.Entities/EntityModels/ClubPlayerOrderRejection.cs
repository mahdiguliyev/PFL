using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class ClubPlayerOrderRejection : BaseModel
    {
        public int Id { get; set; }

        [Column("club_player_order_id")]
        public int ClubPlayerOrderId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("rejection_note")]
        public string RejectionNote { get; set; }


        public virtual ClubPlayerOrder ClubPlayerOrder { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}