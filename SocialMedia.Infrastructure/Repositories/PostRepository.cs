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

        public async Task<bool> UpdatePost(Post post)
        {
            //instaciamos el post currentPost pasandole el id que buscamos y post le pasamos el dato
            var currentPost = await GetPost(post.PostId);
            currentPost.Date = post.Date;
            currentPost.Description = post.Description;
            currentPost.Image = post.Image;
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
            //valida si se afecto la actualización
        }


        public async Task<bool> DeletePost(int id)
        {
            //instaciamos el post currentPost pasandole el id que buscamos y post le pasamos el dato
            var currentPost = await GetPost(id);
            //ayuda a eliminar se debe declarar en la interfaz Task<bool> DeletePost(int id)
            _context.Posts.Remove(currentPost);
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
            //valida si se afecto la actualización
        }


    }
}
