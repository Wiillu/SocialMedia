using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    //se definen los métodos que deben implementar aquellas clases que implementen esta interfaz
    public interface IPostRepository : IRepository<Post>
    {
        /*Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPost(int id);
        //tarea que inserta Post
        Task InsertPost(Post post);

        Task<bool> UpdatePost(Post post);

        Task<bool> DeletePost(int id);*/

        Task<IEnumerable<Post>> GetPostsByUser(int userid);
    }
}
