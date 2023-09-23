using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class ClubTournamentType
    {
        public int Id { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("tournament_type_id")]
        public int TournamentTypeId { get; set; }



        [Column("last_updated_date")]
        public DateTime? LastUpdatedDate { get; set; }

        [Column("last_update_by_id")]
        public Guid? LastUpdateById { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("created_by_id")]
        public Guid? CreatedById { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }


        //public virtual Club Club { get; set; }

        //public virtual TournamentType TournamentType { get; set; }

        //public virtual User UserCreatedBy { get; set; }

        //public virtual User UserLastUpdateBy { get; set; }
    }
}