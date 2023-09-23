using System;

namespace PFL.Models.DTO
{
    public class ClubOfficialOrderDto
    {
        //public int ClubId { get; set; }
        public int ClubOfficialId { get; set; }
        public int ClubTypeId { get; set; }
    }

    public class ClubOfficialOrderListDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string PositionName { get; set; }

        public bool ClubConfirm { get; set; }

        public bool Rejected { get; set; }

        public bool OperatorConfirm { get; set; }

        public DateTime? TerminationDate { get; set; }
        public DateTime? OperatorConfirmDate { get; set; }

    }
}