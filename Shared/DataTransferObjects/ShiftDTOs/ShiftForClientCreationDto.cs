using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftForClientCreationDto
    {
        public string DoctorId { get; init; }
        public DateTime Date { get; init; }
    }
}
