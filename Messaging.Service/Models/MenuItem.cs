// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service.Models
{
    public record MenuItem
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public int Price { get; init; }

        public bool Visible { get; init; } = true;
    }
}
