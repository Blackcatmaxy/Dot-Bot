using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DotBot.Server.Data;
using DotBot.Shared;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotBot.Server.Bot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("withexp")]
        public async Task WithEXP(uint EXP)
        {

        }

        [Command("withlevel")]
        public async Task WithLevel(ushort level)
        {
            await ReplyAsync($"You should need {EXPManager.levelRequirements[level]} EXP for level {level}");
        }

        [Command("ListLevels")]
        public async Task Listlevels()
        {
            var embed = new EmbedBuilder();
            for (int i = 0; i < 5; i++)
            {
                embed.AddField("Level " + i, EXPManager.levelRequirements[i]);
            }
            embed.WithTitle("EXP requirements");
            await ReplyAsync(embed: embed.Build());
        }

        [Command("Myexp")]
        [RequireContext(ContextType.Guild)]
        public async Task ListEXP()
        {
            var user = Context.Guild.GetEXP(Context.User.Id, false).file;

            if (user == null)
            {
                await ReplyAsync("You have no data");
                return;
            }

            await ReplyAsync($"You are level {user.Level} with {user.EXP} XP and {EXPManager.levelRequirements[user.Level + 1] - user.EXP} XP needed to level up!");
        }

        [Command("EXPRoles")]
        [RequireContext(ContextType.Guild)]
        public async Task ListRole()
        {
            var settings = Context.Guild.Get<CommonSettings>(false);
            if (settings == null || !settings.file.roles.Any())
            {
                await ReplyAsync("No roles are set to be given for levels");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithTitle("Roles");
            foreach (EXPRole role in settings.file.roles)
            {
                SocketRole guildRole = Context.Guild.GetRole(role.ID);
                embed.AddField($"Level {role.LevelRequirement}", guildRole.Mention);
            }
            await ReplyAsync(embed: embed.Build());
        }
    }
}
