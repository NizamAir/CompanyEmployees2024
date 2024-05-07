using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? FatherName { get; set; }
        public string? ImageName { get; set; }
        public int? WorkExperience { get; set; }
        public string? Education { get; set; }
        public string? Specialization { get; set; }
        public string? Comment { get; set; }
        public string? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public User? DoctorUser { get; set; }


    }
}
