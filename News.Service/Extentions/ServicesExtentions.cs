// Copyright (c) Fedor Bashilov. All rights reserved.

namespace News.Service.Extentions
{
    using News.Service;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using News.Service.Interfaces;

    public static class ServicesExtentions
    {
        public static void AddNewsServices(this IServiceCollection services)
        {
            services.TryAddSingleton<INewsService, NewsService>();
        }
    }
}
