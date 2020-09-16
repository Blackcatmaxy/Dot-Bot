using DotBot.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DotBot.Server.Data
{
    public class SavedUserEXP : UserEXP
    {
        [BsonId]
        public ulong userID;

        public SavedUserEXP(ulong ID) => userID = ID;
    }
}
