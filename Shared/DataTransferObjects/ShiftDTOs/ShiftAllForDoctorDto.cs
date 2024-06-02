using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftAllForDoctorDto
    {
        public string? AssistantName { get; set; }
        public string ClientName { get; set; }
        public string Date { get; set; }
    }
}
