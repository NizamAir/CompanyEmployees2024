using Shared.DataTransferObjects.ReviewDTOs;

namespace Shared.DataTransferObjects.DoctorDTOs
{
    public record DoctorToReturnDto
    {
        public DoctorDto Doctor { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
        public string Rating {  get; set; } 
    }
}
