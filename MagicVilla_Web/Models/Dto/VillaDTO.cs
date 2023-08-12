﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public int Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string Amenity { get; set; }
    }
}
