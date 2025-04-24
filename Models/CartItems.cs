using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headphones_Webstore.Models
{
    public class CartItems
    {
        [Key]
        public int CartItemID { get; set; }

        [ForeignKey("Session")]
        public required Guid SessionID { get; set; }

        // Навигационное свойство
        public Sessions Session { get; set; }

        [Required]
        public int AnimeId { get; set; } // Было AnimeId

        [ForeignKey("AnimeId")]
        public Anime Anime { get; set; }

        public required int Quantity { get; set; }
        public required DateTime AddedAt { get; set; }
    }
}