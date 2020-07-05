namespace DGBot
{
    using System.ComponentModel.DataAnnotations;
    using CoHAApi;
    using DSharpPlus.Entities;

    public class Tag : IEmbeddable, IModel
    {
        [Key]
        public string Id { get; set; }

        public string Entry { get; set; }

        public Tag(string id, string entry)
        {
            Id = id;
            Entry = entry;
        }

        public DiscordEmbedBuilder ToEmbed()
        {
            var eb = new DiscordEmbedBuilder()
            {
                Title = Id,
                Description = Entry
            };

            return eb;
        }
    }
}