using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.ShiftDTOs
{
    public record ShiftForUpdateDto(DateTime ShiftDate, Guid? ProductId, string DoctorId, string? ClientId, string AssistentId);
}
