using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;

namespace DotBot.Server.Bot
{
    public class BotManager
    {
        public static DiscordSocketClient client;

        public static async Task SetUp()
        {
            var config = new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                ConnectionTimeout = 6000,
                MessageCacheSize = 120,
                ExclusiveBulkDelete = false,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.Guilds | GatewayIntents.DirectMessages
            };
            client = new DiscordSocketClient(config);
            client.Ready += Ready;
            client.Log += Log;
            MessageHandler messageHandler = new MessageHandler(client);
            var serviceConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = true
            };
            CommandService service = new CommandService(serviceConfig);
            CommandHandler handler = new CommandHandler(client, service);
            EXPManager XPManager = new EXPManager(client);
            await handler.InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, HiddenInfo.mainToken);
            await client.StartAsync();
        }

        private static async Task Log(LogMessage message)
        {
            Console.WriteLine(message);
        }

        public static async Task Ready()
        {

        }
    }
}
