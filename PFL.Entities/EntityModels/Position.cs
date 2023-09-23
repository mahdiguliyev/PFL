using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public sealed partial class Position : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Position()
        {
            ClubPlayerRequests = new HashSet<ClubPlayerRequest>();
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Label { get; set; }
    


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Player> Players { get; set; }

        public User User { get; set; }

        public User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequests { get; set; }
    }
}
