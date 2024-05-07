namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftForAssistantCreationDto
    {
        public string DoctorId { get; init; }
        public ICollection<DateOnly> Dates { get; init; }
    }
}
