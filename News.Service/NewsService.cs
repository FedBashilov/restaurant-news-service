// Copyright (c) Fedor Bashilov. All rights reserved.

namespace News.Service
{
    using System.Linq;
    using Infrastructure.Core.Models;
    using Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using News.Service.Exceptions;
    using News.Service.Models.DTOs;

    public class NewsService : INewsService
    {
        private readonly IDbContextFactory<NewsDatabaseContext> dbCxtFactory;

        public NewsService(IDbContextFactory<NewsDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<NewsItem>> GetNews(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var selectQuery = onlyVisible ?
                dbContext.News.Where(x => x.Visible == true) :
                dbContext.News;

            var orderQuery = orderDesc ?
                selectQuery.OrderByDescending(x => x.Id) :
                selectQuery.OrderBy(x => x.Id);

            var pageQuery = orderQuery.Skip(offset).Take(count);

            var news = await pageQuery.ToListAsync();

            return news;
        }

        public async Task<NewsItem> GetNewsItem(int id)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var newsItem = await dbContext.News.FirstAsync(x => x.Id == id);

            if (newsItem == null)
            {
                throw new NotFoundException($"Not found newsItem with id = {id} while executing GetnewsItem method");
            }

            return newsItem;
        }

        public async Task<NewsItem> CreateNewsItem(NewsItemDTO newItemDto)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var newItem = new NewsItem()
            {
                Title = newItemDto.Title,
                Description = newItemDto.Description,
                Visible = newItemDto.Visible,
            };

            var newsItem = dbContext.News.Add(newItem).Entity;
            await dbContext.SaveChangesAsync();

            return newsItem;
        }

        public async Task<NewsItem> UpdateNewsItem(int id, NewsItemDTO newItemDto)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            if (!dbContext.News.Any(x => x.Id == id))
            {
                throw new NotFoundException();
            }

            var newItem = new NewsItem()
            {
                Id = id,
                Title = newItemDto.Title,
                Description = newItemDto.Description,
                Visible = newItemDto.Visible,
            };

            var newsItem = dbContext.News.Update(newItem).Entity;
            await dbContext.SaveChangesAsync();

            return newsItem;
        }
    }
}
