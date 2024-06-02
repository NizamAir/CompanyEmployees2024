namespace Shared.DataTransferObjects.ReviewDTOs
{
    public record ReviewForCreationDto
    {
        public string? Comment { get; set; }
        public int StarsCount { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserWhoRatedId { get; set; }

        public string UserId { get; set; }

        public Guid ShiftId { get; set; }

    }
}
