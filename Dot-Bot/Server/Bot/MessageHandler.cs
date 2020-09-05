using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Discord.WebSocket;
using Discord.Commands;

namespace DotBot.Server.Bot
{
    public class MessageHandler
    {
        public ConcurrentDictionary<ulong, DateTime> lastMessages = new ConcurrentDictionary<ulong, DateTime>();
        private readonly DiscordSocketClient _client;

        public MessageHandler(DiscordSocketClient client)
        {
            _client = client;
            _client.MessageReceived += HandleMessage;

        }

        public async Task HandleMessage(SocketMessage msg) => await Task.Run(() => CheckMessage(msg as SocketUserMessage));

        public async Task CheckMessage(SocketUserMessage message)
        {
            if (message == null) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);

            if (context.Guild == null || message.Author.IsBot) return;

            //Checks dictionary
            if (lastMessages.TryGetValue(message.Author.Id, out DateTime time))
            {
                if (DateTime.UtcNow.Subtract(time) < TimeSpan.FromMinutes(1))
                {
                    return;
                }
            }
            lastMessages[message.Author.Id] = message.Timestamp.UtcDateTime;
            await EXPManager.AddEXP(context);
        }
    }
}
