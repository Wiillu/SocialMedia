using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(SocialMediaContext context) : base(context) //se crea un constructor ya que la clase padre la posee, se recibe el conext y se envía a la clase base con base
        {

        }
        public async Task<IEnumerable<Post>> GetPostsByUser(int userid)
        {
            return await _entities.Where(x => x.UserId == userid).ToListAsync();
        }
    }
}
