using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Shift
    {
        public Guid Id { get; set; }

        public DateTime ShiftDate { get; set; }

        public Guid? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public User? DoctorUser { get; set; }

        public string? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public User? ClientUser { get; set; }

        public string? AssistentId { get; set; }
        [ForeignKey("AssistentId")]
        public User? AssistentUser { get; set; }
    }
}
