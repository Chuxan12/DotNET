using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Headphones_Webstore.Data;
using Headphones_Webstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Headphones_Webstore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddAnimeController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<AnimeController> _logger;

    public AddAnimeController(ApplicationDbContext db, ILogger<AnimeController> logger)
    {
        _db     = db;
        _logger = logger;
    }

    // POST api/addanime
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddAnimeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState); 
        }

        /* ------ простая валидация на стороне сервера (при желании можно углубить) ------ */
        var normalized = dto.Title.Trim().ToLower();
        var exists     = await _db.Anime.AnyAsync(a => a.Title.ToLower() == normalized);
        if (exists)
            return Conflict(new { type = "title", message = "Аниме с таким названием уже существует" });

        if (!Uri.TryCreate(dto.ImageUrl, UriKind.Absolute, out var _))
            return BadRequest(new { type = "imageUrl", message = "Неверный URL изображения" });

        /* ------ создание сущности ------ */
        var anime = new Anime
        {
            Title       = dto.Title.Trim(),
            Rating      = dto.Rating,
            Type        = dto.Type,
            Status      = dto.Status,
            Year        = DateTime.Parse(dto.ReleaseDate).Year,
            ReleaseDate = DateTime.Parse(dto.ReleaseDate),
            Genres      = string.Join(",", dto.Genres.Select(g => g.Trim())),
            Studio      = dto.Studio?.Trim(),
            Description = dto.Description?.Trim(),
            ImagePath   = dto.ImageUrl.Trim(),
            UpdatedAt   = DateTime.UtcNow,
            Episodes    = dto.Episodes
        };

        _db.Anime.Add(anime);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = anime.Id }, new { message = "Аниме добавлено", anime.Id });
    }

    // GET api/anime/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var a = await _db.Anime.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return a is null ? NotFound() : Ok(a);
    }
}

/* ----------- контракт, который будет отправлять фронт ----------- */
public class AddAnimeDto
{
    [Required, StringLength(200)] public string Title        { get; set; } = null!;
    [Range(0, 10)]               public double Rating       { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required]                   public AnimeType Type      { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Required]                   public AnimeStatus Status  { get; set; }
    [MinLength(1)]               public List<string> Genres { get; set; } = new();
    [Required]                   public string ReleaseDate  { get; set; } = null!; // YYYY-MM-DD
    [StringLength(100)]          public string? Studio      { get; set; }
    [StringLength(300)]          public string? Description { get; set; }
    [Required, Url]              public string ImageUrl     { get; set; } = null!;
    [Range(0, 1000)]             public int Episodes        { get; set; } = 0;
}