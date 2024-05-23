namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftForClientCreationDto
    {
        public string DoctorId { get; init; }
        public Guid ProductId { get; init; }
        public DateTime Date { get; init; }
    }
}
