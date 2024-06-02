namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftAllForAssistantDto
    {
        public string DoctorName { get; set; }
        public string Date { get; set; }
    }
}
