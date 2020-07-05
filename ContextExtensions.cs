namespace DGBot
{
    using System.Linq;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Entities;

    public static class ContextExtensions
    {
        public static DiscordChannel GetChannel(this CommandContext ctx, string entry)
        {
            if (ulong.TryParse(entry, out var result))
                return ctx.Guild.Channels.SingleOrDefault(ch => ch.Key  == result).Value;
            else
                return ctx.Guild.Channels.SingleOrDefault(ch => ch.Value.Name.ToLower() == entry.ToLower()).Value;
        }

    }
}