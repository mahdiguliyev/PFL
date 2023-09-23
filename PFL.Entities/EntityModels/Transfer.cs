using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class Transfer : BaseModel

    {
        public int Id { get; set; }

        [Column("club_from_id")]
        public int? ClubFromId { get; set; }

        [Column("club_from_name")]
        [StringLength(200)]
        public string ClubFromName { get; set; }

        [Column("club_to_id")]
        public int? ClubToId { get; set; }

        [Column("club_to_name")]
        [StringLength(200)]
        public string ClubToName { get; set; }

        [Column("player_id")]
        public long? PlayerId { get; set; }

        [Column("contract_type_id")]
        public int? ContractTypeId { get; set; }

        [Column("date_from", TypeName = "date")]
        public DateTime? DateFrom { get; set; }


        [Column("date_to", TypeName = "date")]
        public DateTime? DateTo { get; set; }

        [Column("termination_date")]
        public DateTime? TerminationDate { get; set; }

        [Column("termination_reason")]
        public string TerminationReason { get; set; }


        public virtual ContractType ContractType { get; set; }
        public virtual Club ClubFrom { get; set; }

        public virtual Club ClubTo { get; set; }

        public virtual Player Player { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}