using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Api.models.DTOs
{
    public class WishList
    {
        public int UserWishlistId { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}

