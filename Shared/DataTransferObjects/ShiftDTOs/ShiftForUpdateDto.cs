namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftForUpdateDto(DateTime ShiftDate, Guid? ProductId, string DoctorId, string? ClientId, string AssistentId);
}
