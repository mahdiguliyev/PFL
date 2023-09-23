namespace PFL.Models.ViewModels
{
    public class ClubMatchConfirmViewModel
    {
        ClubMatchConfirmViewModel()
        {

        }
        public ClubMatchConfirmViewModel(int status, string note)
        {
            Status = status;
            Note = note;
        }
        public int Status { get; set; }

        public string Note { get; set; }
    }

    public class PlayerPenaltyStatus
    {
        public int Status { get; set; }

        public string Note { get; set; }
    }

    public class RefereeMatchConfirmViewModel
    {
        public int Status { get; set; }

        public string Note { get; set; }
    }

}