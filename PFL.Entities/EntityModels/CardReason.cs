using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFL.Entities.EntityModels
{
    public sealed class CardReason : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CardReason()
        {
            MatchClubPlayersYellow1 = new HashSet<MatchClubPlayer>();
            MatchClubPlayersYellow2 = new HashSet<MatchClubPlayer>();
            MatchClubPlayersRed = new HashSet<MatchClubPlayer>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }


        public User UserCreatedBy { get; set; }

        public User UserLastUpdatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayersYellow1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayersYellow2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MatchClubPlayer> MatchClubPlayersRed { get; set; }


    }
}