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

                await e.Member.SendMessageAsync(embed: new DiscordEmbedBuilder()
                {
                    Title = "Welcome to the Developer's Guild!",
                    Description = "Please take a moment to introduce yourself in #chat. Let us know" +
                                  "\n\n> What languages and frameworks you're learning/familiar with." +
                                  "\n\n> What Projects you're currently working on." +
                                  "\n\n> What you hope to gain as a member of the community." +
                                  "\n\n> and lastly: A bit about yourself!" + 
                                  "\n\n**Here is an excellent example from one of our new members**" +
                                  "\n\n> Hi everyone! I'm littlebitt. I'm a recent bootcamp graduate with a full stack web developer certification. I also have an associate's degree in information technology. " +
                                  "My core studies programming wise have been on JavaScript things, such as vanilla JavaScript, Angular, React, express JS and nodeJS. " +
                                  "\n\n> Other things I've worked with are: HTML 5, CSS3, MySQL, NoSQL, MongoDB, AWS Deployment on an EC2 server, " +
                                  "PHP, XML, JQuery, Bootstrap (gotta be honest I hate bootstrap), flexbox (this is my primary way of writing my CSS). " +
                                  "\n\n> My current project is an angular personal portfolio that is currently making http requests to two different api's (GitHub and Trello)." +
                                  "If anyone has any ideas, I am all ears. " +
                                  "\n\n> Also, constructive criticism and feedback are most welcome! P.S. it's responsive!"
                });
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