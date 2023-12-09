// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool Visible { get; set; }
    }
}
