using System.ComponentModel.DataAnnotations;

namespace AnimeSite.Models
{
    public class Sessions
    {
        [Key] public required Guid SessionID { get; set; }

        public required DateTime CreatedAt { get; set; }

        // Коллекция связанных CartItems
        public ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
    }
}