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
            var collection = Context.Guild.GetEXPCollection();
            var builder = Builders<SavedUserEXP>.Filter;
            var filter = builder.Eq(doc => doc.userID, Context.User.Id);
            var user = collection.Find(filter).FirstOrDefault();

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
            var collection = Context.Guild.GetSettingsCollection();
            var settings = collection.Get<CommonSettings>("CommonSettings");
            if (settings == null || !settings.roles.Any())
            {
                await ReplyAsync("No roles are set to be given for levels");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithTitle("Roles");
            foreach (EXPRole role in settings.roles)
            {
                SocketRole guildRole = Context.Guild.GetRole(role.ID);
                embed.AddField($"Level {role.LevelRequirement}", guildRole.Mention);
            }
            await ReplyAsync(embed: embed.Build());
        }
    }
}
