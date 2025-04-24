using System.ComponentModel.DataAnnotations;
using Headphones_Webstore.Models;


namespace Headphones_Webstore.Models{
    public class Comment
    {
        public int Id { get; set; }

        public int AnimeId { get; set; }
        public Anime Anime { get; set; } = null!;

        public string Author { get; set; } = "";
        public string Text   { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
