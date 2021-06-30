
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using _10KRanker.Services;

namespace _10KRanker.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("ok!");
        }
    }
}
