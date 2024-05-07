using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string? Comment { get; set; }
        public int StarsCount { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserWhoRatedId { get; set; }
        [ForeignKey("UserWhoRatedId")]
        public User UserWhoRated { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
