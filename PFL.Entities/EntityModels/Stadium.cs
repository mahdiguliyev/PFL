using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    [Table("Stadiums")]
    public class Stadium : BaseModel
    {
        public Stadium()
        {
            Matches = new HashSet<Match>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("is_closed")]
        public bool IsClosed { get; set; }

        public District District { get; set; }

        public ICollection<Match> Matches { get; set; }
        public User UserCreatedBy { get; set; }
        public User UserLastUpdateBy { get; set; }
    }
}