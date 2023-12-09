// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service.Extentions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using News.Service;
    using RabbitMQ.Client;

    public static class ServicesExtentions
    {
        public static void AddMessagingServices(this IServiceCollection services)
        {
            services.AddSingleton<IMenuNewsMessagingReceiver, MenuNewsMessagingReceiver>();
        }
    }
}
