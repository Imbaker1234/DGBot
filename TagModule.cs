namespace DGBot
{
    using System.Text;
    using System.Threading.Tasks;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("tag")]
    [Description("A key-value lookup to store messages for quick recall within Discord.")]
    public class TagModule : BaseCommandModule
    {
        public TagRepository TagRepository { get; set; }

        public TagModule(TagRepository tagRepository)
        {
            TagRepository = tagRepository;
        }

        [GroupCommand]
        [Description("Retrieves a tag (if it exists) matching the provided id")]
        public async Task Tag(CommandContext ctx, [Description("The key for the tag to be retrieved.")] string id)
        {
            var product = await TagRepository.Read(id);

            if (product is null) throw new TagNotFoundException() { TagName = id};

            await ctx.RespondAsync(embed: product.ToEmbed());
        }

        [Command("new")]
        [Description("Enters a new Key-Value lookup in Guildbot's memory.")]
        public async Task Tag(CommandContext ctx, 
            [Description("The id to later retrieve the tag by.")] string id, 
            [Description("The value contained within the tag.")][RemainingText] string entry)
        {
            var tag = new Tag(id, entry);
            var product = await TagRepository.Create(tag);
            
            var eb = new DiscordEmbedBuilder()
                .WithTitle(product.Id)
                .WithDescription(product.Entry);


            await ctx.RespondAsync(embed: eb.Build());
        }

        [Command("edit")]
        [Description(("Updates the value matching the provided id."))]
        public async Task Edit(CommandContext ctx, [Description("The id of the tag to be edited")] string id, 
            [Description("The text to be retrieved via the lookup.")][RemainingText] string entry)
        {
            var tag = new Tag(id, entry);
            var product = await TagRepository.Update(tag);
            
            var eb = new DiscordEmbedBuilder()
                .WithTitle(product.Id)
                .WithDescription(product.Entry);


            await ctx.RespondAsync(embed: eb.Build());
        }
        

        [Command("delete")]
        [Description("Deletes the tag matching the id provided.")]
        public async Task DeleteTag(CommandContext ctx, 
            [Description("The id of the tag being deleted")]string id)
        {
            var product = await TagRepository.Delete(id);
            
            var eb = new DiscordEmbedBuilder()
                .WithTitle(product.Id)
                .WithDescription(product.Entry);

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
            {
                Title = $"Deleted '{product.Id}'",
                Description = "It has been printed one last time for any who may wish to hold on to it"
            });
            await ctx.RespondAsync(embed: eb);
        }
        

        [Command("find")]
        [Description("Looks up all tags with a name matching or like the id provided.")]
        public async Task ReadAll(CommandContext ctx, 
            [Description("The id, in total or in part, of the tag/s to be found (if any)")]string like = null)
        {
            var product = await TagRepository.GetAllLike(like);
            var sb = new StringBuilder();
            
            foreach (var tag in product)
            {
                sb.AppendLine($"- {tag}");
            }

            string title;
            if (like is null) title = "Tags";
            else title = $"Tags for '{like}'";
            
            var eb = new DiscordEmbedBuilder()
                .WithTitle(title)
                .WithDescription(sb.ToString());

            await ctx.RespondAsync(embed: eb);
        }
    }
}