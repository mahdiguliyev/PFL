using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_users_count")]
    public class VUsersCount
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int UserCount { get; set; }
    }
}