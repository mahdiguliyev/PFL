using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed class ClubOfficialOrder : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClubOfficialOrder()
        {
            ClubOfficialOrderRejections = new HashSet<ClubOfficialOrderRejection>();
        }

        public int Id { get; set; }

        [Column("season_id")]
        public int SeasonId { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("club_official_id")]
        public int ClubOfficialId { get; set; }

        public int Year { get; set; }

        [Column("club_type_id")]
        public int ClubTypeId { get; set; }


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



        public ClubOfficial ClubOfficial { get; set; }

        public Club Club { get; set; }

        public ClubType ClubType { get; set; }

        public User UserCreatedBy { get; set; }

        public User UserLastUpdateBy { get; set; }

        public User UserClubConfirmBy { get; set; }

        public User UserOperatorConfirmBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubOfficialOrderRejection> ClubOfficialOrderRejections { get; set; }
    }
}