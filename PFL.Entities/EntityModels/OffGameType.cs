using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public sealed class OffGameType: BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OffGameType()
        {
            MatchClubPlayersYellow = new HashSet<MatchClubPlayer>();
            MatchClubPlayersYellow2 = new HashSet<MatchClubPlayer>();
            MatchClubPlayersRed = new HashSet<MatchClubPlayer>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayersYellow { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayersYellow2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayersRed { get; set; }

        public User UserCreatedBy { get; set; }

        public User UserLastUpdateBy { get; set; }
    }
}