// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Price { get; set; }

        public bool Visible { get; set; } = true;
    }
}
