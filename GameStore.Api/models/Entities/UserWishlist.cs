using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Api.models.Entities
{
    public class UserWishlist
    {
        [Key]
        public int UserWishlistId { get; set; }
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}