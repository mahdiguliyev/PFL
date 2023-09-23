namespace PFL.Models.DTO
{
    public class MatchPlayerShiftDto
    {
        public int ClubId { get; set; }

        public long? PlayerIdOut { get; set; }
        public int? PlayerNumberOut { get; set; }
        public string FirstNameOut { get; set; }
        public string LastNameOut { get; set; }


        public long? PlayerIdIn { get; set; }
        public int? PlayerNumberIn { get; set; }
        public string FirstNameIn { get; set; }
        public string LastNameIn { get; set; }

        public int? Minute { get; set; }
        public int? MinutePlus { get; set; }
    }
}