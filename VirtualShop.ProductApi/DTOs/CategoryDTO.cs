﻿using System.ComponentModel.DataAnnotations;
using VirtualShop.ProductApi.Models;

namespace VirtualShop.ProductApi.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The Name is Required!")]
        [MinLength(3)]
        [MaxLength(100)]
        public string? Name { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
