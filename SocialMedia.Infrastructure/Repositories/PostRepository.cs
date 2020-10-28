using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    // se usa la interfaz IPostRepository
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext _context;
        //usamos constructor para inyectar dependecias de sql server
        public PostRepository(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts;
        }

        public async Task<Post> GetPost(int id)
        {
            //para accesder se debe declarar en la interfaz
            var posts = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);
            return posts;

       }
        //añadir post
        public async Task InsertPost(Post post)
        {
            //guarda post
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

        }




    }
}
