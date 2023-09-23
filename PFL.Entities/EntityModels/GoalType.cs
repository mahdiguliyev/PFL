using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public sealed class GoalType : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoalType()
        {
            MatchResults = new HashSet<MatchResult>();
            MatchPenaltyResults = new HashSet<MatchPenaltyResult>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchResult> MatchResults { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchPenaltyResult> MatchPenaltyResults { get; set; }
    }
}