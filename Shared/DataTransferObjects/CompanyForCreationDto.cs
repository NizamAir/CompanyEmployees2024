using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto
    {
        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Company address is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the address is 20 characters.")]
        public string? Address { get; init; }

        [Required(ErrorMessage = "Company country is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the country is 20 characters.")]
        public string? Country { get; init; }
        
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
    }
}
