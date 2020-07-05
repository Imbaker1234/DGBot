namespace DGBot
{
    using System;
    using System.Linq;
    using DSharpPlus;
    using DSharpPlus.Entities;

    public static class Extensions
    {
        public static DiscordClient HandleUserAdded(this DiscordClient client)
        {
            client.GuildMemberAdded += async e =>
            {
                var accountCreationTime = e.Member.CreationTimestamp;;
                string desc;
                desc = (DateTime.Now - accountCreationTime).TotalDays < 7 ? "They are __new to **Discord**__! Make them feel welcome!" : "Say hello!";

                    await e.Guild.GetChannel(727739183438889020).SendMessageAsync(
                        embed: new DiscordEmbedBuilder()
                        {
                            Title = $"Welcome {e.Member.Username}",
                            Description = desc,
                            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                            {
                                Height = 0,
                                Width = 0,
                                Url = e.Member.AvatarUrl
                            },
                            Footer = new DiscordEmbedBuilder.EmbedFooter()
                            {
                                Text = $"Using Discord since {accountCreationTime}",
                                IconUrl = "http://s0.yellowpages.com.au/589ad8e9-6599-444c-a3a6-04dfd0bf36b0/able-enterprises-maroochydore-4558-accreditation.png"
                            }
                        });

                DiscordRole role = e.Guild.Roles.Values.Single(r => r.Name == "Developer");

                await e.Member.ReplaceRolesAsync(new[] {role});
            };

            return client;
        }
        
        public static DiscordClient HandleUserLeft(this DiscordClient client)
        {
            client.GuildMemberRemoved += async e =>
            {
                var name = !string.IsNullOrEmpty(e.Member.Nickname) ? e.Member.Nickname : e.Member.Username;

                await e.Guild.GetChannel(727739183438889020).SendMessageAsync(
                    embed: new DiscordEmbedBuilder()
                    {
                        Title = $"Fair thee well {name}",
                        Description = "We hardly knew ye",
                        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                        {
                            Height = 0,
                            Width = 0,
                            Url = e.Member.AvatarUrl
                        }
                    });
            };

            return client;
        }
    }
}