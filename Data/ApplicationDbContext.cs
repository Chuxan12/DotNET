using Microsoft.EntityFrameworkCore;
using Headphones_Webstore.Models;

namespace Headphones_Webstore.Data

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 
                   
        }

        public DbSet<Anime> Anime { get; set; } 
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<CartItems> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Session)
                .WithMany(s => s.CartItems)
                .HasForeignKey(ci => ci.SessionID);

            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Anime)
                .WithMany()
                .HasForeignKey(ci => ci.AnimeId);
        }
    }
    public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // убеждаемся, что БД существует и все миграции применены
        context.Database.Migrate();

        // если в таблице аниме уже есть данные — ничего не делаем
        if (context.Anime.Any())
            return;

        // ваши стартовые записи
        var initial = new List<Anime>
        {
            new Anime {
                Title = "Врата Штейна",
                Rating = 9.07,
                Votes = 120000,
                Year = 2011,
                Genres = "Драма,Фантастика,Триллер",
                Studio = "WHITE FOX",
                ImagePath = "img/anime1.jpg",
                Description = "Аниме о путешествиях во времени.",
                ReleaseDate = new DateTime(2011,4,6),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 24,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "За гранью",
                Rating = 7.72, Votes = 45000,
                Year = 2013,
                Genres = "Фэнтези,Приключения",
                Studio = "A-1 Pictures",
                ImagePath = "img/anime2.jpg",
                Description = "Аниме о приведениях",
                ReleaseDate = new DateTime(2013,7,5),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 12,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Горничная",
                Rating = 5.7, Votes = 8000,
                Year = 2010,
                Genres = "Драма,Романтика",
                Studio = "Studio Deen",
                ImagePath = "img/anime3.jpg",
                Description = "Аниме про горничную",
                ReleaseDate = new DateTime(2010,10,2),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 13,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Бездомный бог",
                Rating = 7.94, Votes = 60000,
                Year = 2014,
                Genres = "Ужасы,Триллер",
                Studio = "TMS Entertainment",
                ImagePath = "img/anime4.jpg",
                Description = "Аниме о бездомном боге",
                ReleaseDate = new DateTime(2014,1,6),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 12,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Врата Штейна",
                Rating = 9.07,
                Votes = 120000,
                Year = 2011,
                Genres = "Драма,Фантастика,Триллер",
                Studio = "WHITE FOX",
                ImagePath = "img/anime1.jpg",
                Description = "Аниме о путешествиях во времени.",
                ReleaseDate = new DateTime(2011,4,6),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 24,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "За гранью",
                Rating = 7.72, Votes = 45000,
                Year = 2013,
                Genres = "Фэнтези,Приключения",
                Studio = "A-1 Pictures",
                ImagePath = "img/anime2.jpg",
                Description = "Аниме о приведениях",
                ReleaseDate = new DateTime(2013,7,5),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 12,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Горничная",
                Rating = 5.7, Votes = 8000,
                Year = 2010,
                Genres = "Драма,Романтика",
                Studio = "Studio Deen",
                ImagePath = "img/anime3.jpg",
                Description = "Аниме про горничную",
                ReleaseDate = new DateTime(2010,10,2),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 13,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Бездомный бог",
                Rating = 7.94, Votes = 60000,
                Year = 2014,
                Genres = "Ужасы,Триллер",
                Studio = "TMS Entertainment",
                ImagePath = "img/anime4.jpg",
                Description = "Аниме о бездомном боге",
                ReleaseDate = new DateTime(2014,1,6),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 12,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Врата Штейна",
                Rating = 9.07,
                Votes = 120000,
                Year = 2011,
                Genres = "Драма,Фантастика,Триллер",
                Studio = "WHITE FOX",
                ImagePath = "img/anime1.jpg",
                Description = "Аниме о путешествиях во времени.",
                ReleaseDate = new DateTime(2011,4,6),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 24,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "За гранью",
                Rating = 7.72, Votes = 45000,
                Year = 2013,
                Genres = "Фэнтези,Приключения",
                Studio = "A-1 Pictures",
                ImagePath = "img/anime2.jpg",
                Description = "Аниме о приведениях",
                ReleaseDate = new DateTime(2013,7,5),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 12,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Горничная",
                Rating = 5.7, Votes = 8000,
                Year = 2010,
                Genres = "Драма,Романтика",
                Studio = "Studio Deen",
                ImagePath = "img/anime3.jpg",
                Description = "Аниме про горничную",
                ReleaseDate = new DateTime(2010,10,2),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 13,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            },
            new Anime {
                Title = "Бездомный бог",
                Rating = 7.94, Votes = 60000,
                Year = 2014,
                Genres = "Ужасы,Триллер",
                Studio = "TMS Entertainment",
                ImagePath = "img/anime4.jpg",
                Description = "Аниме о бездомном боге",
                ReleaseDate = new DateTime(2014,1,6),
                UpdatedAt = DateTime.UtcNow,
                Episodes = 12,
                Type = AnimeType.TV,
                Status = AnimeStatus.Finished
            }
        };

        context.Anime.AddRange(initial);
        context.SaveChanges();

        // если нужно — инициализируем комментарии
        var comments = new[]
        {
            new Comment { AnimeId = initial[0].Id, Author = "Иван", Text = "Обожаю этот сериал!", Date = DateTime.UtcNow },
            // ...
        };
        context.Comment.AddRange(comments);
        context.SaveChanges();
    }
}
}
