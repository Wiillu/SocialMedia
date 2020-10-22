using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    //se definen los métodos que deben implementar aquellas clases que implementen esta interfaz
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPosts();
    }
}
