using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public sealed partial class Tournament : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tournament()
        {
            TournamentClubs = new HashSet<TournamentClub>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage="Turnirin adı daxil edilməyib")]
        [StringLength(300)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Turnirin tipi seçilməyib")]
        [Column("tournament_type_id")]
        public int TournamentTypeId { get; set; }

        [Column("season_id")]
        public int SeasonId { get; set; }

        [Required(ErrorMessage = "Başlanğıc il seçilməyib")]
        [Column("season_start_year")]
        public int SeasonStartYear { get; set; }

        [Required(ErrorMessage = "Son il seçilməyib")]
        [Column("season_end_year")]
        public int SeasonEndYear { get; set; }

        [Required(ErrorMessage = "Sarı kart limiti daxil edilmıyib")]
        [Column("yellow_card_limit")]
        public int YellowCardLimit { get; set; }

        [Required(ErrorMessage = "21 yaşlı gənc oyunçu yaş limiti (təvəllüd) seçilməyib")]
        [Column("young_player_age21_limit")]
        public int YoungPlayerAge21Limit { get; set; }

        [Required(ErrorMessage = "21 yaşlı gənc oyunçu yaş limiti (say) seçilməyib")]
        [Column("young_player_age21_count")]
        public int YoungPlayerAge21Count { get; set; }

        [Required(ErrorMessage = "19 yaşlı gənc oyunçu yaş limiti (təvəllüd) seçilməyib")]
        [Column("young_player_age19_limit")]
        public int YoungPlayerAge19Limit { get; set; }

        [Required(ErrorMessage = "19 yaşlı gənc oyunçu yaş limiti (say) seçilməyib")]
        [Column("young_player_age19_count")]
        public int YoungPlayerAge19Count { get; set; }

        [Required(ErrorMessage = "Oyundakı oyunçu sayı daxil edilməyib")]
        [Column("player_count")]
        public int PlayerCount { get; set; }

        [Required(ErrorMessage = "Ehtiyat oyunçu sayı daxil edilməyib")]
        [Column("reserve_player_count")]
        public int ReservePlayerCount { get; set; }

        [Required(ErrorMessage = "Legioner oyunçu say limiti daxil edilməyib")]
        [Column("legioner_limit")]
        public int LegionerLimit { get; set; }

        [Required(ErrorMessage = "Dövrə daxil edilməyib")]
        public int Period { get; set; }

        public int Status { get; set; }

        //[AllowHtml]
        public string Note { get; set; }

        public bool? Completed { get; set; }

        public TournamentType TournamentType { get; set; }

        public User User { get; set; }

        public User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentClub> TournamentClubs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<TournamentTour> TournamentTours { get; set; }
    }
}
