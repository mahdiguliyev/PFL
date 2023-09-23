using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_club_notifications")]
    public class VClubNotification
    {
        [Key]
        public int Id { get; set; }

        public string ClubName { get; set; }

        public int NewOrderCount { get; set; }

        public int ClubPlayerRequestCount { get; set; }

        public int ClubOfficialRequestCount { get; set; }

        public int ClubNewDocumentCount { get; set; }

        public int SeasonId { get; set; }

        public bool IsDeleted { get; set; }
    }
}