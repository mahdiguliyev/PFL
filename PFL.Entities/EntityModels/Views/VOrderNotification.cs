using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_order_notifications")]
    public class VOrderNotification
    {
        [Key]
        public Guid Id { get; set; }
        public string NotificationType { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int NewOrderCount { get; set; }
    }

    [Table("v_club_order_notifications")]
    public class VClubOrderNotification
    {
        [Key]
        public Guid Id { get; set; }
        public string NotificationType { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int NewOrderCount { get; set; }
    }
}