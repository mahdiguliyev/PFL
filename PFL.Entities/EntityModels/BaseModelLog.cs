using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class BaseModelLog : BaseModel
    {
        public int Id { get; set; }

        [Column("main_id")]
        public int MainId { get; set; }

        [Column("log_creation_date")]
        public DateTime LogCreationDate { get; set; }

        [Column("log_created_by_id")]
        public Guid LogCreatedById { get; set; }

        [Column("operation_type_id")]
        public int OperationTypeId { get; set; }
    }
}