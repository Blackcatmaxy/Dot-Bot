using DotBot.Shared;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotBot.Server.Data
{
    public class SaveableCommonSettings : CommonSettings
    {
        [BsonId]
        public const int ID = 1;
    }
}
