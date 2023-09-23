using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public sealed partial class ContractType : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContractType()
        {
            ClubPlayerRequests = new HashSet<ClubPlayerRequest>();
            Players = new HashSet<Player>();
            Transfers = new HashSet<Transfer>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        

        public User User { get; set; }

        public User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Transfer> Transfers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Player> Players { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequest> ClubPlayerRequests { get; set; }
    }
}
