namespace PFL.Models.DTO
{
    public class ClubDocumentDto
    {
        public int DocumentTypeId { get; set; }
        public int DocumentNameId { get; set; }
        public string Label { get; set; }
        public string DocumentName { get; set; }
        public string DocumentExtensions { get; set; }
        public bool IsMultipleFile { get; set; }
        public int? DocumentId { get; set; }
        public int? Year { get; set; }
        public int? ClubId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public bool? ClubConfirm { get; set; }
        public bool? Rejected { get; set; }
        public bool? OperatorConfirm { get; set; }

        public string RejectionNote { get; set; }
    }
}