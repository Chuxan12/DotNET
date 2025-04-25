using AnimeSite.Data;
using AnimeSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest request)
        {
            try
            {
                _logger.LogInformation($"Добавление в корзину аниме {request.AnimeId}");
                var session = await GetOrCreateSessionAsync();
                if (session == null)
                    return BadRequest("Не удалось получить сессию");

                // ищем уже добавленный элемент
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci =>
                        ci.SessionID == session.SessionID &&
                        ci.AnimeId == request.AnimeId);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    _context.CartItems.Update(cartItem);
                }
                else
                {
                    _context.CartItems.Add(new CartItems
                    {
                        SessionID = session.SessionID,
                        AnimeId = request.AnimeId,
                        Quantity = 1,
                        AddedAt = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();

                // возвращаем общее количество элементов в корзине
                var totalItems = await _context.CartItems
                    .Where(ci => ci.SessionID == session.SessionID)
                    .SumAsync(ci => ci.Quantity);

                return Ok(new { totalItems });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении аниме в корзину");
                return StatusCode(500, new
                {
                    error = "Внутренняя ошибка сервера",
                    details = ex.Message
                });
            }
        }

        // GET api/cart/count
        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCount()
        {
            var session = await GetOrCreateSessionAsync();
            if (session == null)
                return Ok(new { totalItems = 0 });

            var totalItems = await _context.CartItems
                .Where(ci => ci.SessionID == session.SessionID)
                .SumAsync(ci => ci.Quantity);

            return Ok(new { totalItems });
        }

        // Получаем или создаём сессию по куки
        private async Task<Sessions?> GetOrCreateSessionAsync()
        {
            // читаем из куки
            var sessionCookie = Request.Cookies["SessionID"];
            if (Guid.TryParse(sessionCookie, out var guid))
            {
                var existing = await _context.Sessions
                    .FirstOrDefaultAsync(s => s.SessionID == guid);
                if (existing != null)
                    return existing;
            }

            // создаём новую
            var session = new Sessions
            {
                SessionID = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            // записываем куку
            Response.Cookies.Append("SessionID", session.SessionID.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });

            return session;
        }
    }

    // DTO для добавления аниме в корзину
    public class CartItemRequest
    {
        // id аниме, которое кладём в корзину
        public int AnimeId { get; set; }
    }
}