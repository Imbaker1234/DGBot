namespace DGBot
{
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public static class PublicService
    {
        public static async Task Thread(
            CommandContext ctx,
            [Description("Name of Category (In quotes) ex. \"General Chat\"")]
            string category,
            [Description("Name of Channel (Dash Separated) ex. \"My-Thoughts\"")]
            string name,
            [Description("Message ID Spawning this Thread.")]
            DiscordMessage message,
            [Description("Large bold text at the head of the new thread.")] [RemainingText]
            string title = "")
        {
            var channel = await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, ctx.GetChannel(category));
            
            await ctx.Channel.SendMessageAsync(
                $"**Original Message:**\n> {ctx.User.Username} has started a new thread. {channel.Mention}");

            await channel.SendMessageAsync(embed: new DiscordEmbedBuilder()
            {
                Title = title,
                Description = message.Content,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    IconUrl = ctx.User.AvatarUrl,
                    Text = $"Thread started by {ctx.User.Username}"
                }
            });
        }
        
        public static async Task Quote(CommandContext ctx, DiscordMessage message)
        {

            var member = await ctx.Guild.GetMemberAsync(message.Author.Id);
            var eb = new DiscordEmbedBuilder()
            {
                Description = message.Content,
                Color = member.Color,
                Author = new DiscordEmbedBuilder.EmbedAuthor()
                {
                    IconUrl = message.Author.AvatarUrl,
                    Name = message.Author.Username
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    IconUrl = "https://cdn.dribbble.com/users/2101163/screenshots/4215462/outback-logo_1x.png",
                    Text = "This message brought to you by Outback Steakhouse"
                },
            }.WithThumbnail(ctx.Message.Author.AvatarUrl);
            await ctx.Channel.SendMessageAsync(embed: eb.Build());
        }
    }
}