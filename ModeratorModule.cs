using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DGBot
{
    [RequireRoles(RoleCheckMode.Any, "Trustee")]
    public class ModeratorCommands : BaseCommandModule
    {
        [Command("Ban")]
        public async Task Ban(CommandContext ctx, DiscordUser user, string reason = "")
        {
            await ModeratorService.Ban(ctx, user, reason);
        }


        [Command("Archive")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        public async Task Archive(CommandContext ctx, [RemainingText] string reason)
        {
            await ModeratorService.Archive(ctx, reason);
        }

        [Command("Move")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        public async Task Move(
            CommandContext ctx,
            DiscordChannel channel,
            string category,
            string reason,
            string moveTitle = "Channel Moved")
        {
            await ModeratorService.Move(ctx, channel, category, reason, moveTitle);
        }

        [Command("clear")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        [Description("Clears chat")]
        public async Task Clear(CommandContext ctx, ulong startId, ulong endId)
        {
            await ModeratorService.Clear(ctx, startId, endId);
        }

        [Command("SyncPermissions")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        public async Task SyncPermissions(CommandContext ctx, DiscordChannel channel, DiscordChannel category = null)
        {
            await ModeratorService.SyncPermissions(ctx, channel, category);
        }
    }
}