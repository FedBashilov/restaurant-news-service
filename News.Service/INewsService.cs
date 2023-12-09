// Copyright (c) Fedor Bashilov. All rights reserved.

namespace News.Service
{
    using Infrastructure.Core.Models;
    using News.Service.Models.DTOs;

    public interface INewsService
    {
        public Task<List<NewsItem>> GetNews(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true);

        public Task<NewsItem> GetNewsItem(int id);

        public Task<NewsItem> CreateNewsItem(NewsItemDTO newsItem);

        public Task<NewsItem> UpdateNewsItem(int id, NewsItemDTO newsItem);
    }
}
