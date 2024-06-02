using Shared.DataTransferObjects.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.DoctorDTOs
{
    public record DoctorDto(Guid id, string FirstName, string LastName, string FatherName, int WorkExperience, string Education, string Specialization, string Comment, string ImageName);
}
