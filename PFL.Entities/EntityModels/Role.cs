using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public partial class Role : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            //UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }


        public virtual ICollection<User> Users { get; set; }
    }
}
