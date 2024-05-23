namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftDoctorNameDto
    {
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
