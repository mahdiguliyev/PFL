using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class ClubOfficialRejection : BaseModel
    {
        public int Id { get; set; }

        [Column("club_official_id")]
        public int ClubOfficialId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("rejection_note")]
        public string RejectionNote { get; set; }


        public virtual ClubOfficial ClubOfficial { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdate { get; set; }
    }
}