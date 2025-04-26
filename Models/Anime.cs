using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeSite.Models
{
    public enum AnimeType
    {
        TV,
        Movie,
        OVA,
        ONA,
        Special
    }

    public enum AnimeStatus
    {
        Airing,
        Finished,
        Upcoming
    }

    public class Anime
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(200)] public string Title { get; set; } = null!;

        [Range(0, 10)] public double Rating { get; set; }

        public int Votes { get; set; } = 0;

        public int Year { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Genres { get; set; } = string.Empty;

        [StringLength(100)] public string? Studio { get; set; }

        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int Episodes { get; set; } = 0;

        public AnimeType Type { get; set; } = AnimeType.TV;

        public AnimeStatus Status { get; set; } = AnimeStatus.Finished;
    }

    public class AnimeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public double Rating { get; set; }
        public int Votes { get; set; }
        public int Year { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public string? Studio { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Episodes { get; set; }
        public AnimeType Type { get; set; }
        public AnimeStatus Status { get; set; }
    }
}