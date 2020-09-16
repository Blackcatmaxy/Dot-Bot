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

        [Command("addrole")]
        public async Task AddRole(SocketRole role, byte level)
        {
            var settings = Context.Guild.Get<CommonSettings>(true);
            if (settings.file.roles.Any(r => r.ID == role.Id))
            {
                await ReplyAsync("Role is already included");
                return;
            }
            settings.file.roles.Add(new EXPRole { ID = role.Id, LevelRequirement = (byte)level });
            settings.Save();
            await ReplyAsync($"Role {role.Name} will now be given out to people who reach level {level} when they level up");
        }
    }
}
