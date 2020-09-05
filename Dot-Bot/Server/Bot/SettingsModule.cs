using Discord.Commands;
using Discord.WebSocket;
using DotBot.Server.Data;
using DotBot.Shared;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotBot.Server.Bot
{
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
        [Command("setinvite")]
        public async Task SetInvite()
        {

        }

        //[Command("addrole")]
        //public async Task AddRole(SocketRole role, byte level)
        //{
        //    var collection = Context.Guild.GetSettingsCollection();
        //    var settings = collection.GetCommonSettings() ?? new SaveableCommonSettings();
        //    if (settings.roles.Any(r => r.ID == role.Id))
        //    {
        //        await ReplyAsync("Role is already included");
        //        return;
        //    }
        //    settings.roles.Add(new EXPRole { ID = role.Id, LevelRequirement = (byte)level });
        //    //var document = BsonMapper.Global.ToDocument<CommonSettings>(settings);
        //    collection.Upsert(SaveableCommonSettings.ID, settings);
        //    await ReplyAsync($"Role {role.Name} will now be given out to people who reach level {level} when they level up");
        //}
    }
}
