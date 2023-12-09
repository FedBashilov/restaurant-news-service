// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    using Messaging.Service.Models;
    using News.Service;
    using News.Service.Models.DTOs;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using System.Text.Json;

    public class MenuNewsMessagingReceiver : IMenuNewsMessagingReceiver
    {
        private readonly INewsService newsService;

        public MenuNewsMessagingReceiver(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public void Subscribe()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueBind("menu_to_news_queue", "menu_to_news_exchange", "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var menuItem = JsonSerializer.Deserialize<MenuItem>(message);

                var newsItemDto = new NewsItemDTO()
                {
                    Title = $"New on the menu! Try {menuItem?.Name}",
                    Description = $"Here you can try the new delicious {menuItem?.Name} for only {menuItem?.Price}!",
                    Visible = true
                };


                await this.newsService.CreateNewsItem(newsItemDto);
            };
            channel.BasicConsume("menu_to_news_queue", true, consumer);
        }
    }
}