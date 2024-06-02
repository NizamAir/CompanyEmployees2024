namespace Shared.DataTransferObjects.StatisticsDTOs
{
    public record DoctorStatDto
    {
        public string Id { get; set; }
        public List<DoctorStatDtoCoordinats> Data {  get; set; } 
    }
}
