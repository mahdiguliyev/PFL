using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public partial class UserRole : BaseModel
    {
        public int Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }



        public virtual Role Role { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
