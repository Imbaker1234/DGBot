namespace DGBot
{
    using System;
    using System.Threading.Tasks;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using HelpPlus;

    public class PublicCommands : BaseCommandModule
    {
        [Command("Thread")]
        [HelpDescription("Creates a new channel for a dedicated discussion to a singular topic.",
            "/thread \"General Chat\" My-Thoughts 723584685703168061 Incoming Rant Alert!",
            "/thread \"Ice Cream Aficionados\" Chocolate-Or-Vanilla 723584685703168061 I mean...We all know the answer here right?")]
        [HelpThumb("https://i.ibb.co/Xx5n7nG/Red-Thread.png")]
        public async Task Thread(
            CommandContext ctx,
            [HelpDescription("Name of the Category (In Quotes)", "\"General Chat\"")]
            string category,
            [HelpDescription("Name of Channel (Dash Separated as Appropriate)", "My-Thoughts")]
            string name,
            [HelpDescription("Message ID Spawning this Thread. You can get this by right clicking the message",
                "723604681405890580")]
            DiscordMessage message,
            [HelpDescription("Large bold text at the head of the new thread.", "This is what this is all about")]
            [RemainingText]
            string title = "")
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