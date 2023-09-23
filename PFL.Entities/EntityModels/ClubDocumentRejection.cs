using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class ClubDocumentRejection : BaseModel
    {
        public int Id { get; set; }

        [Column("club_document_id")]
        public int ClubDocumentId { get; set; }

        [Column("rejection_note")]
        [Required]
        [StringLength(500)]
        public string RejectionNote { get; set; }


        public virtual ClubDocument ClubDocument { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}
