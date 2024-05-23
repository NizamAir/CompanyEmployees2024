using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.DoctorDTOs
{
    public record DoctorForUpdateDto { 
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FatherName { get; init; }
        public int WorkExperience { get; init; }
        public string Education { get; init; }
        public string Specialization { get; init; }
        public string Comment { get; init; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; init; }
    }
    
}
