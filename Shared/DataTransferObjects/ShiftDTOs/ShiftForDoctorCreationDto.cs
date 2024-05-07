using Microsoft.VisualBasic;

namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftForDoctorCreationDto
    {
        public ICollection<DateOnly> Dates { get; init; }

    }
}
