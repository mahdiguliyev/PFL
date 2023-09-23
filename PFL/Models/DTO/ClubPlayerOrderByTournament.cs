namespace PFL.Models.DTO
{
    public class ClubPlayerOrderByTournament
    {
        public int ClubId { get; set; }
        public long PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public int PlayerNumber { get; set; }
    }
}