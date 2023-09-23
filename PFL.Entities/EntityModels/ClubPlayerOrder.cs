using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed class ClubPlayerOrder : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClubPlayerOrder()
        {
            ClubPlayerOrderRejections = new HashSet<ClubPlayerOrderRejection>();
        }

        public int Id { get; set; }

        [Column("season_id")]
        public int SeasonId { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("player_id")]
        public long PlayerId { get; set; }

        [Column("player_number")]
        public int PlayerNumber { get; set; }

        public int Year { get; set; }

        [Column("club_type_id")]
        public int ClubTypeId { get; set; }

        [Column("health_file")]
        public string HealthFile { get; set; }


        [Column("club_confirm")]
        public bool ClubConfirm { get; set; }

        [Column("club_confirm_by_id")]
        public Guid? ClubConfirmById { get; set; }

        [Column("club_confirm_date")]
        public DateTime? ClubConfirmDate { get; set; }

        public bool Rejected { get; set; }

        [Column("operator_confirm")]
        public bool OperatorConfirm { get; set; }

        [Column("operator_confirm_by_id")]
        public Guid? OperatorConfirmById { get; set; }

        [Column("operator_confirm_date")]
        public DateTime? OperatorConfirmDate { get; set; }


        public Club Club { get; set; }
        public Player Player { get; set; }
        public ClubType ClubType { get; set; }

        public User UserCreatedBy { get; set; }

        public User UserLastUpdateBy { get; set; }

        public User UserClubConfirmBy { get; set; }

        public User UserOperatorConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerOrderRejection> ClubPlayerOrderRejections { get; set; }
    }
}