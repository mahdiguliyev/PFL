namespace PFL.Models.DTO
{
    public class TransferDto
    {
        public long PlayerId { get; set; }
        public int ClubFromId { get; set; }
        public string ClubFromName { get; set; }
        public int ClubToId { get; set; }
    }
}