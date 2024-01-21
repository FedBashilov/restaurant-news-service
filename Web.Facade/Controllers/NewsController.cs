// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Core.Exceptions;
    using Infrastructure.Core.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using News.Service.Interfaces;
    using News.Service.Models.DTOs;
    using Web.Facade.Models.Responses;

    [Route("api/v1/news")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService newsService;
        private readonly IStringLocalizer<NewsController> localizer;
        private readonly ILogger<NewsController> logger;

        public NewsController(
            INewsService newsService,
            IStringLocalizer<NewsController> localizer,
            ILogger<NewsController> logger)
        {
            this.newsService = newsService;
            this.localizer = localizer;
            this.logger = logger;
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(List<NewsItem>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetNews(
            [FromQuery][Range(0, int.MaxValue)] int offset = 0,
            [FromQuery][Range(1, int.MaxValue)] int count = 100,
            [FromQuery] bool orderDesc = false,
            [FromQuery] bool onlyVisible = true)
        {
            try
            {
                var news = await this.newsService.GetNews(offset, count, orderDesc, onlyVisible);
                return this.Ok(news);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't get news. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(NewsItem))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetNewsItem([FromRoute] int id)
        {
            try
            {
                var newsItem = await this.newsService.GetNewsItem(id);
                return this.Ok(newsItem);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't get news item. Not found news item with id = {id}.");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't get news item. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("")]
        [ProducesResponseType(201, Type = typeof(NewsItem))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateNewsItem([FromBody] NewsItemDTO newsItemDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            try
            {
                var newsItem = await this.newsService.CreateNewsItem(newsItemDto);
                return this.StatusCode(201, newsItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't create news item. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(NewsItem))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateNewsItem([FromRoute] int id, [FromBody] NewsItemDTO newsItemDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            try
            {
                var newsItem = await this.newsService.UpdateNewsItem(id, newsItemDto);
                return this.Ok(newsItem);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update news item. Not found news item with id = {id}.");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't update news item. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        private bool IsInputModelValid([NotNullWhen(false)]out string? errorMessage)
        {
            if (!this.ModelState.IsValid)
            {
                errorMessage = this.ModelState
                    .SelectMany(state => state.Value!.Errors)
                    .Aggregate(string.Empty, (current, error) => current + (error.ErrorMessage + ". "));

                return false;
            }

            errorMessage = null;

            return true;
        }
    }
}
