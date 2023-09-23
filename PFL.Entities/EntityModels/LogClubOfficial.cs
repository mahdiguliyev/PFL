using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed class LogClubOfficial : LogBaseModel
    {
        public int Id { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Column("last_name")]
        public string LastName { get; set; }

        [StringLength(100)]
        [Column("father_name")]
        public string FatherName { get; set; }

        [Required]
        [Column("position_id")]
        public int PositionId { get; set; }

        [Column("contract_begin_date")]
        public DateTime? ContractBeginDate { get; set; }

        [Column("contract_end_date")]
        public DateTime? ContractEndDate { get; set; }

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Column("termination_date")]
        public DateTime? TerminationDate { get; set; }

        [StringLength(500)]
        [Column("termination_reason")]
        public string TerminationReason { get; set; }

        [Column("citizenship_id")]
        public int? CitizenshipId { get; set; }

        [StringLength(100)]
        public string Contact { get; set; }

        [StringLength(500)]
        [Column("photo_url")]
        public string PhotoUrl { get; set; }

        [StringLength(500)]
        [Column("passport_url")]
        public string PassportUrl { get; set; }

        [StringLength(500)]
        [Column("contract_url")]
        public string ContractUrl { get; set; }

        [StringLength(500)]
        [Column("trainer_license_url")]
        public string TrainerLicenseUrl { get; set; }

        [StringLength(500)]
        [Column("doctor_diploma_url")]
        public string DoctorDiplomaUrl { get; set; }

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
    }
}