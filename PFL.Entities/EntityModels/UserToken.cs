using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public partial class UserToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Token { get; set; }

        [Column("token_creation_date")]
        public DateTime TkenCreationDate { get; set; }

        [Column("token_update_date")]
        public DateTime? TokenUpdateDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
    }
}
