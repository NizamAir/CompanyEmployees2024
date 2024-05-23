using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.DoctorDTOs
{
    public record DoctorForInitialDto(string FirstName, string LastName, string doctorId);
}
