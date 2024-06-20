using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Api.Models.DTOs
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Navigation to products
        public ICollection<Product>? Products { get; set; }

        public static implicit operator Category?(string? v)
        {
            throw new NotImplementedException();
        }
    }
}