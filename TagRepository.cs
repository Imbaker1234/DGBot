namespace DGBot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoHAPersistence;
    using Microsoft.EntityFrameworkCore;

    public class TagRepository : EntityRepository<Tag>
    {
        public TagRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<string>> GetAllLike(string like = null)
        {
            IEnumerable<string> product; 
            if (like != null)
            {
                product = await 
                    DbContext.Set<Tag>().Where(t => t.Id.Contains(like))
                    .Select(t => t.Id).ToListAsync();
            }
            else
            {
                product = await 
                    DbContext.Set<Tag>().Select(t => t.Id).ToListAsync();
            }

            return product;
        }
    }
}