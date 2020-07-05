namespace DGBot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public static class ConfigRepository
    {
        static GuildBotDbContext GuildBotDbContext { get; set; }

        public static async Task<Tag> Create(Tag tag)
        {
            EntityEntry<Tag> product;
            await using (var transaction = await GuildBotDbContext.Database.BeginTransactionAsync())
            {
                product = await GuildBotDbContext.AddAsync(tag);

                await GuildBotDbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return product.Entity;
        }

        public static async Task<Tag> Read(string id)
        {
            Tag product;

            await using (var transaction = await GuildBotDbContext.Database.BeginTransactionAsync())
            {
                product = await GuildBotDbContext.Set<Tag>().AsQueryable().SingleOrDefaultAsync(t => t.Id.ToLower() == id.ToLower());
            }

            return product;
        }

        public static async Task<Tag> Delete(string id)
        {
            Tag product;
            using (var transaction = await GuildBotDbContext.Database.BeginTransactionAsync())
            {
                product = await GuildBotDbContext.FindAsync<Tag>(id);
                GuildBotDbContext.Remove(product);
                await GuildBotDbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return product;
        }

        public static async Task<IEnumerable<Tag>> ReadAll(string like = null)
        {
            IEnumerable<Tag> product;

            using (var transaction = await GuildBotDbContext.Database.BeginTransactionAsync())
            {
                if (like != null)
                    product = GuildBotDbContext.Set<Tag>().AsQueryable().Where(t => t.Id.ToLower().Contains(like.ToLower())).ToList();
                else
                {
                    product = GuildBotDbContext.Set<Tag>().AsQueryable().ToList();
                }
            }

            return product;
        }

    }
}