using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed class ClubDocumentName : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClubDocumentName()
        {
            ClubDocuments = new HashSet<ClubDocument>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Label { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }


        [Column("club_document_type_id")]
        public int ClubDocumentTypeId { get; set; }

        [Column("document_extensions")]
        [StringLength(100)]
        public string DocumentExtentions { get; set; }

        [Column("is_multiple_file")]
        public bool IsMultipleFile { get; set; }

        
        public int? Sort { get; set; }


        public ClubDocumentType ClubDocumentType { get; set; }

        public User UserCreatedBy { get; set; }

        public User UserLastUpdateBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocument> ClubDocuments { get; set; }
    }
}
