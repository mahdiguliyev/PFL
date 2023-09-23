using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed partial class Referee : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Referee()
        {
            MatchesReferee = new HashSet<Match>();
            MatchesRefereeAssistant1 = new HashSet<Match>();
            MatchesRefereeAssistant2 = new HashSet<Match>();
            MatchesFourthReferee = new HashSet<Match>();
            MatchesAdditionalReferee1 = new HashSet<Match>();
            MatchesAdditionalReferee2 = new HashSet<Match>();
            MatchesAffaRepresentative = new HashSet<Match>();
            MatchesRefereeInspector = new HashSet<Match>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Column("father_name")]
        public string FatherName { get; set; }

        [StringLength(500)]
        public string Grade { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("referee_type_id")]
        public int RefereeTypeId { get; set; }

        [Column("is_closed")]
        public bool IsClosed { get; set; }


        public RefereeType RefereeType { get; set; }
        public District District { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesReferee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesRefereeAssistant1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesRefereeAssistant2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesFourthReferee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesAdditionalReferee1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesAdditionalReferee2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesRefereeVar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesRefereeAvar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesAffaRepresentative { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Match> MatchesRefereeInspector { get; set; }
    }
}