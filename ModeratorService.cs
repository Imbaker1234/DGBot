namespace DGBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public class ModeratorService
    {
        public async Task Ban(CommandContext ctx, DiscordUser user, string reason = "")
        {
            if (string.IsNullOrEmpty(reason))
            {
                var sender = await ctx.Guild.GetMemberAsync(ctx.User.Id);

                var errorEb = new DiscordEmbedBuilder()
                {
                    Title = "When Banning a User you **must** provide a reason",
                    Description =
                        "When banning a user you must provide a reason.\n\n**Format:** /ban @username reason\n\n**Example:** /ban @TyphoonGoon Repeated rudeness after numerous warnings.",
                    Color = DiscordColor.Red
                };
                await sender.SendMessageAsync(embed: errorEb.Build());
                return;
            }

            var banhammer = DiscordEmoji.FromGuildEmote(ctx.Client, 720115559366393898);

            var channel = await ctx.Guild.CreateChannelAsync
                ($"Ban {user.Username}", ChannelType.Text, ctx.Guild.GetChannel(720327050635772044));

            var eb = new DiscordEmbedBuilder();
            eb.Description =
                $"**{DiscordEmoji.FromGuildEmote(ctx.Client, 720115559366393898)}{ctx.User.Mention} has initiated proceedings to ban {user.Mention} {DiscordEmoji.FromGuildEmote(ctx.Client, 720115559366393898)}**" +
                "\n\nThis user has been globally muted until such time as a decision has been rendered." +
                "\n\nTrustees may react below to support the ban for this individual";

            eb.Color = DiscordColor.Teal;
            eb.Footer = new DiscordEmbedBuilder.EmbedFooter()
            {
                Text = $"{user.Id}",
                IconUrl = user.AvatarUrl
            };

            var message = await channel.SendMessageAsync(embed: eb.Build());

            await message.PinAsync();
            await message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(ctx.Client, 720115559366393898));

            var member = await ctx.Guild.GetMemberAsync(user.Id);

            await ctx.Guild.BanMemberAsync(member, 0, reason);
        }

        public async Task Archive(CommandContext ctx, [RemainingText] string reason)
        {
            await Move(ctx, ctx.Channel, "Archive", reason);
        }

        public async Task Move(
            CommandContext ctx,
            DiscordChannel channel,
            string category,
            string reason,
            string moveTitle = "Channel Moved")
        {
            try
            {
                await ctx.Channel.ModifyAsync(model => model.Parent = ctx.GetChannel(category));
            }
            catch (Exception e)
            {
                await ctx.Member.SendMessageAsync($"Failed to move channel {channel.Name} to {category}: {e}");
            }

            var eb = new DiscordEmbedBuilder
            {
                Title = $"**{moveTitle}**",
                Footer = new DiscordEmbedBuilder.EmbedFooter() {Text = $"Reason: {reason}"}
            };

            await SyncPermissions(ctx, ctx.Channel);

            await ctx.Channel.SendMessageAsync(embed: eb.Build());
        }

        // public async Task Clear(CommandContext ctx, DiscordMessage start, DiscordMessage end)
        // {
        //     var last100 = await ctx.Channel.GetMessagesAsync();
        //
        //     bool net = false;
        //     bool release = false;
        //     var messagesToBeDeleted = new List<DiscordMessage>();
        //     foreach (var discordMessage in last100)
        //     {
        //         if (discordMessage.Id == end.Id) net = true;
        //         if (net)
        //         {
        //             messagesToBeDeleted.Add(discordMessage);
        //         }
        //
        //         if (discordMessage.Id == start.Id) release = true;
        //         if(release) break;
        //     }
        //     
        //     if(!release) throw new Exception("Could not finding starting message by its ID in this channel.");
        //     if(!net) throw new Exception("Could not finding ending message by its ID in this channel.");
        //     
        //     await ctx.Channel.DeleteMessagesAsync(messagesToBeDeleted);
        //
        //     var vanisher = await ctx.RespondAsync("", false, new DiscordEmbedBuilder()
        //         .WithAuthor(ctx.Member.DisplayName, null, ctx.Member.AvatarUrl)
        //         .WithColor(DiscordColor.SpringGreen)
        //         .WithDescription($"Cleared {messagesToBeDeleted.Count} messages!")
        //         .WithFooter(ctx.Client.CurrentUser.Username, ctx.Client.CurrentUser.AvatarUrl));
        //
        //     await Task.Delay(5 * 1000);
        //
        //     await vanisher.DeleteAsync();
        // }

        [Command("Clear")]
        public async Task Clear(CommandContext ctx, ulong startId, ulong endId)
        {
            var start = await ctx.Channel.GetMessageAsync(startId);
            var end = await ctx.Channel.GetMessageAsync(endId);

            if(start is null) throw new Exception("Could not find starting message in this channel.");
            if(end is null) throw new Exception("Could not find ending message in this channel.");
            
            var last100 = await ctx.Channel.GetMessagesBeforeAsync(end.Id, 100);

            var msgs = from msg in last100
                where msg.Timestamp >= start.Timestamp
                group msg by msg.CreationTimestamp > DateTimeOffset.Now.AddDays(-14) ? true : false into grpedMsgs
                select grpedMsgs;

            if(msgs.Any(x => x.Key == true)) 
                await ctx.Channel.DeleteMessagesAsync(msgs.Where(x => x.Key == true).SelectMany(x => x));

            if(msgs.Any(x => x.Key == false))
            {
                foreach (var rm in msgs.Where(x => x.Key == false))
                {
                    foreach (var msg in rm)
                    {
                        await msg.DeleteAsync();
                    }
                }
            }

            var vanisher = await ctx.RespondAsync("", false, new DiscordEmbedBuilder()
                .WithAuthor(ctx.Member.DisplayName, null, ctx.Member.AvatarUrl)
                .WithColor(DiscordColor.SpringGreen)
                .WithDescription($"Cleared {msgs.Count()} messages!")
                .WithFooter(ctx.Client.CurrentUser.Username, ctx.Client.CurrentUser.AvatarUrl));

            await Task.Delay(5 * 1000);

            await vanisher.DeleteAsync();
        }

        public async Task SyncPermissions(CommandContext ctx, DiscordChannel channel,
            DiscordChannel category = null)
        {
            var parent = category ?? channel.Parent;
            if (parent.Type != ChannelType.Category)
            {
                //TODO Throw custom exception
            }

            foreach (var ow in channel.Parent.PermissionOverwrites)
            {
                var role = await ow.GetRoleAsync();
                await channel.AddOverwriteAsync(role, ow.Allowed, ow.Denied,
                    $"Syncing with Parent per request from {ctx.User}");
            }
        }
    }
}