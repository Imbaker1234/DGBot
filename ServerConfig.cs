namespace DGBot
{
    using System.ComponentModel.DataAnnotations;

    public class ServerConfig
    {
        [Key]
        public ulong ServerId { get; set; }
        public bool DeleteCommandPrompts { get; set; }
        public ulong ArchiveCategoryId { get; set; }
        public ulong ModeratorChatId { get; set; }
    }
}