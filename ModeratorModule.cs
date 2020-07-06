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
        private ModeratorService Service { get; set; }

        public ModeratorCommands(ModeratorService service)
        {
            Service = service;
        }

        [Command("Ban")]
        public async Task Ban(CommandContext ctx, DiscordUser user, string reason = "")
        {
            await Service.Ban(ctx, user, reason);
        }


        [Command("Archive")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        public async Task Archive(CommandContext ctx, [RemainingText] string reason)
        {
            await Service.Archive(ctx, reason);
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
            await Service.Move(ctx, channel, category, reason, moveTitle);
        }

        [Command("clear")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        [Description("Clears chat")]
        public async Task Clear(CommandContext ctx, ulong startId, ulong endId)
        {
            await Service.Clear(ctx, startId, endId);
        }

        [Command("SyncPermissions")]
        [RequireRoles(RoleCheckMode.Any, "Trustee")]
        public async Task SyncPermissions(CommandContext ctx, DiscordChannel channel, DiscordChannel category = null)
        {
            await Service.SyncPermissions(ctx, channel, category);
        }
    }
}