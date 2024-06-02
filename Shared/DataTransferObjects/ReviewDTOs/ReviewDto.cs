namespace Shared.DataTransferObjects.ReviewDTOs
{
    public record ReviewDto(Guid Id, string CreationDate, string CreationTime, int StarsCount, Guid ShiftId, string Comment, string ClientName);
}
