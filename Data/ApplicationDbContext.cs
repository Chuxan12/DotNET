using AnimeSite.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeSite.Data

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Anime> Anime { get; set; }
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
                new Anime
                {
                    Title = "Атака титанов",
                    Rating = 9.08,
                    Votes = 2000000,
                    Year = 2013,
                    Genres = "Экшен,Драма,Фэнтези",
                    Studio = "Wit Studio / MAPPA",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/10/47347.jpg",
                    Description =
                        "Человечество скрывается за гигантскими стенами, спасаясь от титанов-людоедов. Молодой солдат Эрен Йегер клянётся уничтожить их всех.",
                    ReleaseDate = new DateTime(2013, 04, 07),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 59,
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Стальной алхимик: Братство",
                    Rating = 9.13,
                    Votes = 1800000,
                    Year = 2009,
                    Genres = "Приключения,Драма,Фэнтези",
                    Studio = "Bones",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/1223/96541.jpg",
                    Description =
                        "Братья Элрик ищут Философский камень, чтобы вернуть потерянные тела, нарушив главный запрет алхимии.",
                    ReleaseDate = new DateTime(2009, 04, 05),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 64,
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Тетрадь смерти",
                    Rating = 8.63,
                    Votes = 2100000,
                    Year = 2006,
                    Genres = "Триллер,Сёнэн",
                    Studio = "Madhouse",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/9/9453.jpg",
                    Description =
                        "Старшеклассник Лайт Ягами находит тетрадь, способную убивать любого, чьё имя будет записано в ней.",
                    ReleaseDate = new DateTime(2006, 10, 04),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 37,
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "One Piece",
                    Rating = 8.74,
                    Votes = 3000000,
                    Year = 1999,
                    Genres = "Приключения,Комедия,Фэнтези",
                    Studio = "Toei Animation",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/6/73245.jpg",
                    Description =
                        "Юный пират Манки Д. Луффи отправляется за легендарным сокровищем «One Piece», чтобы стать королём пиратов.",
                    ReleaseDate = new DateTime(1999, 10, 20),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 1000, // обновите при необходимости
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Airing
                },
                new Anime
                {
                    Title = "Клинок, рассекающий демонов",
                    Rating = 8.51,
                    Votes = 950000,
                    Year = 2019,
                    Genres = "Экшен,Фэнтези,Сёнэн",
                    Studio = "ufotable",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/1286/99889.jpg",
                    Description =
                        "Тандзиро вступает в отряд истребителей демонов, чтобы спасти сестру, превращённую в демона.",
                    ReleaseDate = new DateTime(2019, 04, 06),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 26, // TV-сезон 1; можно расширить
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Airing
                },
                new Anime
                {
                    Title = "Охотник х Охотник (2011)",
                    Rating = 9.05,
                    Votes = 1200000,
                    Year = 2011,
                    Genres = "Приключения,Фэнтези,Сёнэн",
                    Studio = "Madhouse",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/11/33657.jpg",
                    Description =
                        "Мальчик Гон хочет стать Хантером, чтобы найти своего отца, одного из величайших охотников.",
                    ReleaseDate = new DateTime(2011, 10, 02),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 148,
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Naruto: Shippuuden",
                    Rating = 8.23,
                    Votes = 1700000,
                    Year = 2007,
                    Genres = "Сенен,Экшен,Приключения",
                    Studio = "Studio Pierrot",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/5/17407.jpg",
                    Description =
                        "Спустя два года Наруто возвращается в деревню, чтобы продолжить путь ниндзя и спасти друга.",
                    ReleaseDate = new DateTime(2007, 02, 15),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 500,
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Jujutsu Kaisen",
                    Rating = 8.64,
                    Votes = 800000,
                    Year = 2020,
                    Genres = "Экшен,Триллер",
                    Studio = "MAPPA",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/1171/109222.jpg",
                    Description =
                        "Итадори Юдзи поглощает проклятый палец демона Рёмена Судзу, чтобы спасти друзей, и вступает в мир магов-шаманистов.",
                    ReleaseDate = new DateTime(2020, 10, 03),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 47, // S1 + S2
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Airing
                },
                new Anime
                {
                    Title = "Spy × Family",
                    Rating = 8.42,
                    Votes = 550000,
                    Year = 2022,
                    Genres = "Комедия,Семейный",
                    Studio = "Wit Studio / CloverWorks",
                    ImagePath = "https://img.yani.tv/posters/full/1636693264.jpg",
                    Description =
                        "Супершпион «Сумрак» собирает фиктивную семью, не зная, что жена — убийца, а дочь — телепат.",
                    ReleaseDate = new DateTime(2022, 04, 09),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 37, // 1-2 сезоны
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Airing
                },
                new Anime
                {
                    Title = "Chainsaw Man",
                    Rating = 8.61,
                    Votes = 650000,
                    Year = 2022,
                    Genres = "Экшен,Триллер",
                    Studio = "MAPPA",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/1806/126216.jpg",
                    Description =
                        "Безродный Денджи заключает контракт со своим питомцем-демоном Почитой и превращается в человека-бензопилу.",
                    ReleaseDate = new DateTime(2022, 10, 12),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 12,
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Airing
                },
                new Anime
                {
                    Title = "Mushoku Tensei: Jobless Reincarnation",
                    Rating = 8.37,
                    Votes = 450000,
                    Year = 2021,
                    Genres = "Фэнтези,Исекай,Драма",
                    Studio = "Studio Bind",
                    ImagePath = "https://img.yani.tv/posters/full/1636691706.jpg",
                    Description =
                        "Тридцатилетний неудачник перерождается в магическом мире, решив на этот раз прожить жизнь без сожалений.",
                    ReleaseDate = new DateTime(2021, 01, 11),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 23, // TV-1; укажите новое число при продлении
                    Type = AnimeType.TV,
                    Status = AnimeStatus.Airing
                },
                new Anime
                {
                    Title = "Твоё имя",
                    Rating = 8.87,
                    Votes = 2100000,
                    Year = 2016,
                    Genres = "Драма,Романтика,Фэнтези",
                    Studio = "CoMix Wave Films",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/5/87048.jpg",
                    Description =
                        "Двое старшеклассников из разных уголков Японии внезапно начинают менятьcя телами во сне и пытаются встретиться в реальности.",
                    ReleaseDate = new DateTime(2016, 08, 26),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 1,
                    Type = AnimeType.Movie,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Унесённые призраками",
                    Rating = 8.85,
                    Votes = 1600000,
                    Year = 2001,
                    Genres = "Приключения,Сказка",
                    Studio = "Studio Ghibli",
                    ImagePath = "https://cdn.myanimelist.net/images/anime/6/79597.jpg",
                    Description =
                        "10-летняя Тихиро попадает в мир духов и должна спасти родителей, превращённых в свиней.",
                    ReleaseDate = new DateTime(2001, 07, 20),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 1,
                    Type = AnimeType.Movie,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Дитя погоды",
                    Rating = 8.32,
                    Votes = 640000,
                    Year = 2019,
                    Genres = "Романтика,Фэнтези,Драма",
                    Studio = "CoMix Wave Films",
                    ImagePath =
                        "https://shikimori.one/uploads/poster/animes/38826/d9c3e31fccb2802c259b735c3a3655ef.jpeg",
                    Description =
                        "Беглый школьник встречает девушку-«солнечную деву», способную рассеивать дождь над Токио.",
                    ReleaseDate = new DateTime(2019, 07, 19),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 1,
                    Type = AnimeType.Movie,
                    Status = AnimeStatus.Finished
                },

                /* ------------► ДОБАВЛЯЕМ СПЕШЛЫ ◄------------ */
                new Anime
                {
                    Title = "Death Note: Rewrite",
                    Rating = 7.40,
                    Votes = 260000,
                    Year = 2007,
                    Genres = "Триллер,Сверхъестественное",
                    Studio = "Madhouse",
                    ImagePath = "https://imgos.info/upload/anime/images/5a3d50ed7460b.jpg",
                    Description = "Двухчасовой спецрелиз — пересказ событий сериала от лица Рюка с новыми сценами.",
                    ReleaseDate = new DateTime(2007, 08, 31),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 1,
                    Type = AnimeType.Special,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "Attack on Titan: No Regrets",
                    Rating = 8.36,
                    Votes = 550000,
                    Year = 2014,
                    Genres = "Экшен,Фэнтези",
                    Studio = "Wit Studio",
                    ImagePath =
                        "https://shikimori.one/uploads/poster/animes/25781/649324642e35b46236a89a85e1f092d9.jpeg",
                    Description = "Двухсерийный приквел о прошлом Леви и его пути в Разведкорпус.",
                    ReleaseDate = new DateTime(2014, 12, 09),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 2,
                    Type = AnimeType.Special,
                    Status = AnimeStatus.Finished
                },
                new Anime
                {
                    Title = "One Piece: Episode of East Blue",
                    Rating = 7.66,
                    Votes = 120000,
                    Year = 2017,
                    Genres = "Приключения,Комедия",
                    Studio = "Toei Animation",
                    ImagePath = "https://static.wikia.nocookie.net/onepiece/images/f/f6/Episode_of_East_Blue.png",
                    Description = "Фильм-спешл, пересказывающий раннюю сагу East Blue с новым рисунком и музыкой.",
                    ReleaseDate = new DateTime(2017, 08, 26),
                    UpdatedAt = DateTime.UtcNow,
                    Episodes = 1,
                    Type = AnimeType.Special,
                    Status = AnimeStatus.Finished
                }
            };

            context.Anime.AddRange(initial);
            context.SaveChanges();
        }
    }
}