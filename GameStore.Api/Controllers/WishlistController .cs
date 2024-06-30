using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.Api.models.DTOs;
using GameStore.Api.models.Entities;
using GameStore.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireUserRole")]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WishlistController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Wishlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishList>>> GetWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlists = await _context.UserWishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                .ToListAsync();

            var wishlistDtos = wishlists.Select(w => new WishList
            {
                UserWishlistId = w.UserWishlistId,
                UserId = w.UserId,
                User = w.User,
                ProductId = w.ProductId,
                Product = w.Product
            }).ToList();

            return Ok(wishlistDtos);
        }

        // GET: api/Wishlist/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WishList>> GetWishlistItem(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = await _context.UserWishlists
                .Where(w => w.UserId == userId && w.UserWishlistId == id)
                .Include(w => w.Product)
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                return NotFound();
            }

            var wishlistDto = new WishList
            {
                UserWishlistId = wishlist.UserWishlistId,
                UserId = wishlist.UserId,
                User = wishlist.User,
                ProductId = wishlist.ProductId,
                Product = wishlist.Product
            };

            return wishlistDto;
        }

        // POST: api/Wishlist/{id}/wishlist
        [HttpPost("{id}/wishlist")]
        public async Task<ActionResult<WishList>> AddToWishlist(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var wishlist = new UserWishlist
            {
                UserId = userId,
                ProductId = id,
                Product = product
            };

            _context.UserWishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            var wishlistDto = new WishList
            {
                UserWishlistId = wishlist.UserWishlistId,
                UserId = wishlist.UserId,
                ProductId = wishlist.ProductId,
                Product = wishlist.Product
            };

            return CreatedAtAction(nameof(GetWishlistItem), new { id = wishlistDto.UserWishlistId }, wishlistDto);
        }

        // DELETE: api/Wishlist/{id}/wishlist
        [HttpDelete("{id}/wishlist")]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = await _context.UserWishlists
                .Where(w => w.UserId == userId && w.ProductId == id)
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                return NotFound();
            }

            _context.UserWishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}