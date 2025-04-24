using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Headphones_Webstore.Data;
using Headphones_Webstore.Models;

namespace Headphones_Webstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 8; // единый источник истины

        public AnimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* =====================================================================
         * GET: api/anime?page=1&searchTerm=...&genres=Action,!Comedy&sortBy=Rating&order=desc
         * ---------------------------------------------------------------------
         * ➤ 2025‑04‑25  — FIX
         *   1) Поддержка "!genre" для исключения жанров
         *   2) Параметры sortBy / order (Title | ReleaseDate | Rating, asc|desc)
         * ===================================================================*/
        [HttpGet]
        public async Task<IActionResult> GetAnimes(
            [FromQuery] int page = 1,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? genres = null,
            [FromQuery] AnimeType? type = null,
            [FromQuery] AnimeStatus? status = null,
            [FromQuery] int? minYear = null,
            [FromQuery] int? maxYear = null,
            [FromQuery] double? minRating = null,
            [FromQuery] double? maxRating = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order = "asc")
        {
            if (page < 1)
                return BadRequest("Page must be at least 1.");

            var query = _context.Set<Anime>().AsQueryable();

            /* ------------------------------ ФИЛЬТРЫ ------------------------------ */
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(a => EF.Functions.Like(a.Title, $"%{term}%"));
            }

            if (!string.IsNullOrWhiteSpace(genres))
            {
                var tokens = genres.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(g => g.Trim())
                                    .ToList();

                var includeGenres = tokens.Where(g => !g.StartsWith("!", StringComparison.Ordinal)).ToList();
                var excludeGenres = tokens.Where(g => g.StartsWith("!", StringComparison.Ordinal))
                                           .Select(g => g[1..])
                                           .ToList();

                if (includeGenres.Any())
                {
                    query = query.Where(a => includeGenres.Any(g => a.Genres.Contains(g)));
                }
                if (excludeGenres.Any())
                {
                    query = query.Where(a => !excludeGenres.Any(g => a.Genres.Contains(g)));
                }
            }

            if (type.HasValue)
                query = query.Where(a => a.Type == type.Value);

            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            if (minYear.HasValue)
                query = query.Where(a => a.Year >= minYear.Value);
            if (maxYear.HasValue)
                query = query.Where(a => a.Year <= maxYear.Value);

            if (minRating.HasValue)
                query = query.Where(a => a.Rating >= minRating.Value);
            if (maxRating.HasValue)
                query = query.Where(a => a.Rating <= maxRating.Value);

            /* ----------------------------- СОРТИРОВКА ----------------------------- */
            bool desc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
            switch (sortBy?.ToLower())
            {
                case "title":
                    query = desc ? query.OrderByDescending(a => a.Title) : query.OrderBy(a => a.Title);
                    break;
                case "releasedate":
                case "released":    // на всякий случай
                    query = desc ? query.OrderByDescending(a => a.ReleaseDate) : query.OrderBy(a => a.ReleaseDate);
                    break;
                case "rating":
                    query = desc ? query.OrderByDescending(a => a.Rating) : query.OrderBy(a => a.Rating);
                    break;
                default:
                    query = desc ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }

            /* ----------------------------- ПАГИНАЦИЯ ------------------------------ */
            int totalAnimes = await query.CountAsync();
            int totalPages = totalAnimes > 0 ? (int)Math.Ceiling(totalAnimes / (double)PageSize) : 0;

            var animes = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(a => new AnimeDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Rating = a.Rating,
                    Votes = a.Votes,
                    Year = a.Year,
                    Genres = a.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Studio = a.Studio,
                    ImagePath = a.ImagePath,
                    Description = a.Description,
                    ReleaseDate = a.ReleaseDate,
                    UpdatedAt = a.UpdatedAt,
                    Episodes = a.Episodes,
                    Type = a.Type,
                    Status = a.Status
                })
                .ToListAsync();

            return Ok(new
            {
                TotalAnimes = totalAnimes,
                Page = page,
                PageSize,
                TotalPages = totalPages,
                Items = animes
            });
        }

        // ---------------------------------------------------------------------
        // GET: api/anime/suggestions?searchTerm=naruto
        // ---------------------------------------------------------------------
        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Ok(Array.Empty<object>());

            var normalized = searchTerm.Trim().ToLower();

            var list = await _context.Set<Anime>()
                .Where(a => EF.Functions.Like(a.Title.ToLower(), normalized + "%"))
                .OrderBy(a => a.Title)
                .Select(a => new { a.Id, a.Title })
                .Take(5)
                .ToListAsync();

            return Ok(list);
        }

        // ---------------------------------------------------------------------
        // GET: api/anime/5
        // ---------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _context.Set<Anime>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (a == null)
                return NotFound(new { error = "Anime not found" });

            var dto = new AnimeDto
            {
                Id = a.Id,
                Title = a.Title,
                Rating = a.Rating,
                Votes = a.Votes,
                Year = a.Year,
                Genres = a.Genres.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                Studio = a.Studio,
                ImagePath = a.ImagePath,
                Description = a.Description,
                ReleaseDate = a.ReleaseDate,
                UpdatedAt = a.UpdatedAt,
                Episodes = a.Episodes,
                Type = a.Type,
                Status = a.Status
            };

            return Ok(dto);
        }
    }
}
