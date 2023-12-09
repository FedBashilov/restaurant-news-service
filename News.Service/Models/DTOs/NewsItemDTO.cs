// Copyright (c) Fedor Bashilov. All rights reserved.

namespace News.Service.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class NewsItemDTO
    {
        [Required(ErrorMessage = "The Title param is required")]
        public string? Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool Visible { get; set; } = true;
    }
}
