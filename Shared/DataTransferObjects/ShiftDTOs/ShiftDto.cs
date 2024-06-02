namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftDto(Guid Id, string ShiftDate, string ShiftTime, string DoctorName, string AssistantName, string ClientName,string ProductName);
}
