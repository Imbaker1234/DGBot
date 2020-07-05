namespace DGBot
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public class GuildBotDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        private string _db { get; set; }

        protected GuildBotDbContext(DbSet<Tag> tags, string db)
        {
            Tags = tags;
            _db = db;
        }

        public GuildBotDbContext(string db)
        {
            _db = db;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_db}", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}