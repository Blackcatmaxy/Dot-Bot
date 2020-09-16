using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotBot.Server.Data.Models
{
    public class DataObject<TFile, TDocument>
    {
        //Collections of the same type can be specified (like <UserEXP>) while collection that contain different ones can just be <BSONDocument>
        public IMongoCollection<TDocument> collection;

        //This will probably annoy (since instead of settings.foo = bar you'll need to do settings.file.foo = bar), but needs to be done to allow the leader board to work easier
        //Partial class declarations across assemblies/projects could've done same thing but aren't possible
        public TFile file;
    }
}
