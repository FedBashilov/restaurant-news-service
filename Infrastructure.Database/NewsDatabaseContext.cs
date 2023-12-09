// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Database
{
    using Infrastructure.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public class NewsDatabaseContext : DbContext
    {
        public NewsDatabaseContext(DbContextOptions<NewsDatabaseContext> options)
            : base(options) => this.Database.EnsureCreated();

        public DbSet<NewsItem> News => this.Set<NewsItem>();
    }
}