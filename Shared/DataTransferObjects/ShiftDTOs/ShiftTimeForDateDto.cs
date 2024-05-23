namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftTimeForDateDto
    {
        public string DoctorId { get; init; }
        public string Date { get; init; }
    }
}
