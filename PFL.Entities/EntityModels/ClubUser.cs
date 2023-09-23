using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class ClubUser : BaseModel
    {
        public int Id { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        public virtual Club Club { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}