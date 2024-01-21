// Copyright (c) Fedor Bashilov. All rights reserved.

namespace News.Service.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public record NewsItemDTO
    {
        [Required(ErrorMessage = "The Title param is required")]
        public string? Title { get; init; }

        public string Description { get; init; } = string.Empty;

        public bool Visible { get; init; } = true;
    }
}
