using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed class ClubPlayerRequest : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClubPlayerRequest()
        {
            ClubPlayerRequestRejections = new HashSet<ClubPlayerRequestRejection>();
        }
        public int Id { get; set; }

        [Column("season_id")]
        public int SeasonId { get; set; }

        [Column("request_club_id")]
        public int RequestClubId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("player_first_name")]
        public string PlayerFirstName { get; set; }

        [StringLength(100)]
        [Column("player_last_name")]
        public string PlayerLastName { get; set; }

        [StringLength(100)]
        [Column("player_father_name")]
        public string PlayerFatherName { get; set; }
        
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("citizenship_id")]
        public int CitizenshipId { get; set; }

        [Column("contract_type_id")]
        public int ContractTypeId { get; set; }

        [Column("from_club_id")]
        public int? FromClubId { get; set; }

        [StringLength(100)]
        [Column("from_club_name")]
        public string FromClubName { get; set; }

        [Column("contract_begin_date")]
        public DateTime ContractBeginDate { get; set; }

        [Column("contract_end_date")]
        public DateTime ContractEndDate { get; set; }

        [Column("player_number")]
        public int PlayerNumber { get; set; }

        [Column("position_id")]
        public int PositionId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("player_photo_url")]
        public string PlayerPhotoUrl { get; set; }

        [Required]
        [StringLength(500)]
        [Column("player_pasport_url")]
        public string PlayerPasportUrl { get; set; }

        [Required]
        [StringLength(500)]
        [Column("player_contract_url")]
        public string PlayerContractUrl { get; set; }

        //[Required]
        [StringLength(500)]
        [Column("player_otkripleniya_url")]
        public string PlayerOtkripleniyaUrl { get; set; }

        //[Required]
        [StringLength(500)]
        [Column("player_tms_url")]
        public string PlayerTmsUrl { get; set; }

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

      


        public Club RequestClub { get; set; }

        public Club FromClub { get; set; }

        public User UserClubConfirmBy { get; set; }

        public User UserOperatorConfirmBy { get; set; }

        public User UserLastUpdateBy { get; set; }

        public User UserCreatedBy { get; set; }

        public ContractType ContractType { get; set; }

        public Country Country { get; set; }

        public Position Position { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubPlayerRequestRejection> ClubPlayerRequestRejections { get; set; }
    }
}