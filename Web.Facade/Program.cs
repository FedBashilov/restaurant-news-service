// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade
{
    using Messaging.Service.Interfaces;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var menuNewsMessagingReceiver = host.Services.GetRequiredService<IMenuNewsMessagingReceiver>();
            menuNewsMessagingReceiver.Subscribe();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}