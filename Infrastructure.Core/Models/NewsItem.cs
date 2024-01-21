// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    public record NewsItem
    {
        public int Id { get; init; }

        public string? Title { get; init; }

        public string? Description { get; init; }

        public bool Visible { get; init; }
    }
}
