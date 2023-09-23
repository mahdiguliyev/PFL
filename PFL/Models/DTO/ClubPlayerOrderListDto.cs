using System;

namespace PFL.Models.DTO
{
    public class ClubPlayerOrderListDto
    {
        public int Id { get; set; }

        public int PlayerNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }

        public string HealthFile { get; set; }
        public bool ClubConfirm { get; set; }

        public bool Rejected { get; set; }

        public bool OperatorConfirm { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public DateTime? TerminationDate { get; set; }
        public DateTime? OperatorConfirmDate { get; set; }

    }
}