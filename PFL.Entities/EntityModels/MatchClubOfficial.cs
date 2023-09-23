using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class MatchClubOfficial : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MatchClubOfficial()
        {
            OfficialTournamentPenalties = new HashSet<OfficialTournamentPenalty>();
        }

        public int Id { get; set; }

        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }

        [Column("club_official_id")]
        public int ClubOfficialId { get; set; }

        [Column("club_type_id")]
        public int ClubTypeId { get; set; }

        [Column("match_official_zone_type_id")]
        public int MatchOfficialZoneTypeId { get; set; }

        public virtual ClubOfficial ClubOfficial { get; set; }

        public virtual Club Club { get; set; }

        public virtual ClubType ClubType { get; set; }

        public virtual Match Match { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OfficialTournamentPenalty> OfficialTournamentPenalties { get; set; }
    }
}