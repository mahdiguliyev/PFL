using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class BaseModel
    {
        [Column("last_updated_date")]
        public DateTime? LastUpdatedDate { get; set; }

        [Column("last_update_by_id")]
        public Guid? LastUpdateById { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("created_by_id")]
        public Guid CreatedById { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}