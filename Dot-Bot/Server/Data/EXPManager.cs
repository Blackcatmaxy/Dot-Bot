using Discord.Commands;
using Discord.WebSocket;
using DotBot.Server.Data;
using DotBot.Shared;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace DotBot.Server
{
    public class EXPManager
    {
        public static readonly uint[] levelRequirements = new uint[121];
        private DiscordSocketClient _client;
        private static MongoClient dbClient;
        public static IMongoDatabase guildDataBase;
        public static IMongoDatabase EXPDataBase;

        public EXPManager(DiscordSocketClient client)
        {
            BsonClassMap.RegisterClassMap<SavedUserEXP>();
            BsonClassMap.RegisterClassMap<CommonSettings>();

            dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            guildDataBase = dbClient.GetDatabase("EXPSettings");
            EXPDataBase = dbClient.GetDatabase("EXPData");
            _client = client;
            uint lastLevelReq = 0;
            for (int i = 0; i < 120; i++)
            {
                var newLevelReq = 5 * (i * i) + 50 * i + 100;
                uint currentLevelReq = lastLevelReq + (uint)newLevelReq;
                levelRequirements[i + 1] = currentLevelReq;
                lastLevelReq = currentLevelReq;
            }
        }

        public static async Task AddEXP(SocketCommandContext context)
        {
            var collection = context.Guild.GetEXPCollection();
            var builder = Builders<SavedUserEXP>.Filter;
            var filter = builder.Eq(doc => doc.userID, context.User.Id);
            var user = collection.Find(filter).FirstOrDefault()
                ?? new SavedUserEXP(context.User.Id);

            user.EXP += 20;
            user.MessageCount++;
            if (user.EXP >= levelRequirements[user.Level + 1])
            {
                user.Level++;
                await context.Channel.SendMessageAsync($"Congrats {context.User.Mention} for leveling up!");
            }

            collection.ReplaceOne(filter, user, new ReplaceOptions { IsUpsert = true });
        }
    }
}
