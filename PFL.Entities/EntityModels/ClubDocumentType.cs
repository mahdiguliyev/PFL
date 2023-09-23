using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public sealed class ClubDocumentType : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClubDocumentType()
        {
            ClubDocumentNames = new HashSet<ClubDocumentName>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentName> ClubDocumentNames { get; set; }

        public User UserCreatedBy { get; set; }

        public User UserLastUpdateBy { get; set; }
    }
}
