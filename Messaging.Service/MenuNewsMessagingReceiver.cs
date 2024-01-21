// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    using Messaging.Service.Interfaces;
    using Messaging.Service.Models;
    using Messaging.Service.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using News.Service.Interfaces;
    using News.Service.Models.DTOs;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using System.Text.Json;

    public class MenuNewsMessagingReceiver : IMenuNewsMessagingReceiver
    {
        private readonly INewsService newsService;
        private readonly RabbitMqSettings rbMqSettings;
        private readonly ILogger<MenuNewsMessagingReceiver> logger;

        public MenuNewsMessagingReceiver(
            INewsService newsService,
            IOptions<RabbitMqSettings> rbMqSettings,
            ILogger<MenuNewsMessagingReceiver> logger)
        {
            this.newsService = newsService;
            this.rbMqSettings = rbMqSettings.Value;
            this.logger = logger;
        }

        public void Subscribe()
        {
            var factory = new ConnectionFactory
            {
                HostName = this.rbMqSettings.HostName,
                UserName = this.rbMqSettings.UserName,
                Password = this.rbMqSettings.UserPassword,
                Port = this.rbMqSettings.HostPort,
            };

            try
            {
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueBind(this.rbMqSettings.QueueName, this.rbMqSettings.ExchangeName, "");

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

                channel.BasicConsume(this.rbMqSettings.QueueName, true, consumer);
            }
            catch (Exception e)
            {
                this.logger.LogError("RabbitMQ operation failed!" + e.Message);
            }
        }
    }
}