using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using book_store_be.Repositories;
using Microsoft.AspNetCore.Mvc;
using book_store_be.Models;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public UsersController(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    [HttpPost("{bookId}/cart")]
    public async Task<IActionResult> AddBookToCart([FromRoute] Guid bookId, [FromBody] TokenModel tokenModel)
    {
        try
        {
            var userId = GetUserIdFromToken(tokenModel.Token);
            var user = await _repository.AddBookToCartAsync(userId, bookId);
            return Ok(user.Cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{bookId}/cart")]
    public async Task<IActionResult> RemoveBookFromCart([FromRoute] Guid bookId, [FromBody] TokenModel tokenModel)
    {
        try
        {
            var userId = GetUserIdFromToken(tokenModel.Token);
            var user = await _repository.RemoveBookFromCartAsync(userId, bookId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("cart")]
    public async Task<IActionResult> GetUserCart([FromBody] TokenModel tokenModel)
    {
        try
        {
            var userId = GetUserIdFromToken(tokenModel.Token);
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user.Cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("clearcart")]
    public async Task<IActionResult> ClearUserCart([FromBody] TokenModel tokenModel)
    {
        try
        {
            var userId = GetUserIdFromToken(tokenModel.Token);
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Cart.Clear();
            await _repository.UpdateUserAsync(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("cart/count")]
    public async Task<IActionResult> GetCartItemCount([FromQuery] string token)
    {
        try
        {
            var userId = GetUserIdFromToken(token);
            var count = await _repository.GetCartItemCountAsync(userId);
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile([FromQuery] string token)
    {
        try
        {
            var userId = GetUserIdFromToken(token);
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private int GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return int.Parse(userIdClaim.Value);
    }
}
