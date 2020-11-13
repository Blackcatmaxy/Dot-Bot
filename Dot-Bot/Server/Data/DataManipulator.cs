using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DotBot.Server.Data.Models;
using DotBot.Shared;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DotBot.Server.Data
{
    public static class DataManipulator
    {
        public static IMongoCollection<SavedUserEXP> GetEXPCollection(this SocketGuild guild)
        {
            return EXPManager.EXPDataBase.GetCollection<SavedUserEXP>(guild.Id.ToString());
        }

        public static DataObject<T, BsonDocument> Get<T>(this SocketGuild guild, bool createFile) where T : class
            => guild.Id.Get<T>(createFile);

        public static DataObject<TFile, BsonDocument> Get<TFile>(this ulong guildID, bool createFile) where TFile : class
        {
            var collection = EXPManager.guildDataBase.GetCollection<BsonDocument>(guildID.ToString());

            TFile file = null;
            var filter = Builders<BsonDocument>.Filter.Eq("_id", typeof(TFile).Name);
            using (var cursor = collection.Find(filter).ToCursor())
            {
                var doc = cursor?.FirstOrDefault();
                if (doc != null)
                {
                    //needs to be removed since we can't tell the deserializer to not make an error when there's extra elements without making the shared project have the driver package
                    doc.Remove("_id");
                    file = BsonSerializer.Deserialize<TFile>(doc);
                }
            }

            if (createFile)
                file ??= (TFile)Activator.CreateInstance(typeof(TFile));

            if (file == null) return null;

            return new DataObject<TFile, BsonDocument>() { collection = collection, file = file };
        }

        public static DataObject<SavedUserEXP, SavedUserEXP> GetEXP(this SocketGuild guild, ulong userID, bool createFile)
            => guild.Id.GetEXP(userID, createFile);

        public static DataObject<SavedUserEXP, SavedUserEXP> GetEXP(this ulong guildID, ulong userID, bool createFile)
        {
            var collection = EXPManager.EXPDataBase.GetCollection<SavedUserEXP>(guildID.ToString());

            var builder = Builders<SavedUserEXP>.Filter;
            var filter = builder.Eq(doc => doc.userID, userID);
            var user = collection.Find(filter).FirstOrDefault();
            if (createFile)
                user ??= new SavedUserEXP(userID);

            if (user == null) return null;

            return new DataObject<SavedUserEXP, SavedUserEXP>() { collection = collection, file = user };
        }

        public static void Save<T>(this DataObject<T, BsonDocument> dataObject)
        {
            var document = dataObject.file.ToBsonDocument();
            string typeName = typeof(T).Name;
            //ID is just added as part of the document so we don't have to include it in JSON communication with client when building leaderboard
            if (!typeof(T).IsAssignableFrom(typeof(SavedUserEXP))) //UserEXP uses user IDs for data storage so we need to make sure not to override that
                document.Set("_id", typeName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", typeName);
            dataObject.collection.ReplaceOne(filter, document, new ReplaceOptions { IsUpsert = true });
        }
    }
}