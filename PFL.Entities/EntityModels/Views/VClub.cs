using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_clubs")]
    public class VClub
    {
        [Key]
        public int Id { get; set; }

        public string ClubName { get; set; }

        public int NewOrderCount { get; set; }

        public int ClubPlayerRequestCount { get; set; }

        public int ClubOfficialRequestCount { get; set; }

        public bool IsDeleted { get; set; }
    }
}