using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed class ClubDocument : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClubDocument()
        {
            ClubDocumentRejections = new HashSet<ClubDocumentRejection>();
        }

        public int Id { get; set; }

        [Column("season_id")]
        public int SeasonId { get; set; }

        public int Year { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("club_document_name_id")]
        public int ClubDocumenNameId { get; set; }

        [Column("file_path")]
        [Required]
        [StringLength(300)]
        public string FilePath { get; set; }


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



        public ClubDocumentName ClubDocumentName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ClubDocumentRejection> ClubDocumentRejections { get; set; }

        public Club Club { get; set; }

        public User UserCreatedBy { get; set; }

        public User UserLastUpdateBy { get; set; }
    }
}
