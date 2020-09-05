using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DotBot.Shared;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DotBot.Server.Data
{
    public static class DataManipulator
    {
        private const string letters = "ABCDEFGHIJ";

        public static string ToLetters(this ulong number)
        {
            string result = "";
            foreach (char c in number.ToString())
            {
                var digit = byte.Parse(c.ToString());
                result += letters[digit];
            }
            return result;
        }

        public static IMongoCollection<SavedUserEXP> GetEXPCollection(this SocketGuild guild)
        {
            return EXPManager.EXPDataBase.GetCollection<SavedUserEXP>(guild.Id.ToString());
        }

        public static IMongoCollection<BsonDocument> GetSettingsCollection(this SocketGuild guild)
        {
            return EXPManager.guildDataBase.GetCollection<BsonDocument>(guild.Id.ToString());
        }

        //Could do more with polymorphism but we'll probably only have two different settings classes so doesn't matter
        public static CommonSettings GetCommonSettings(this IMongoCollection<BsonDocument> collection)
        {
            var document = collection.Get<CommonSettings>("CommonSettings");
            return document;
            //return BsonMapper.Global.Deserialize<SaveableCommonSettings>(document);
        }

        public static T Get<T>(this IMongoCollection<BsonDocument> collection, object ID, bool createFile = false) where T : class
        {
            T file = null;
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ID.ToString());
            using (var cursor = collection.Find(filter).ToCursor())
            {
                var doc = cursor?.FirstOrDefault();
                if (doc != null) file = BsonSerializer.Deserialize<T>(doc);
            }
            if (createFile && file == null)
            {
                file = (T)Activator.CreateInstance(typeof(T));
                return file;
            }

            return file;
        }
    }
}