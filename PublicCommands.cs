namespace DGBot
{
    using System.Threading.Tasks;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public class PublicCommands : BaseCommandModule
    {
        [Command("Thread")]
        [Description("Creates a new channel for a dedicated discussion to a singular topic.")]
        public async Task Thread(
            CommandContext ctx,
            [Description("Name of the Category (In Quotes)")] string category,
            [Description("Name of Channel (Dash Separated as Appropriate)")] string name,
            [Description("Message ID Spawning this Thread. You can get this by right clicking the message")]
            DiscordMessage message,
            [Description("Large bold text at the head of the new thread.")] [RemainingText] string title = "")
        {
            await PublicService.Thread(ctx, category, name, message, title);
        }

        [Command("Quote")]
        public async Task Quote(CommandContext ctx, [Description("Id of the message to quote")] DiscordMessage message)
        {
            await PublicService.Quote(ctx, message);
        }
    }
}