using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    /*public class PostMongoRepository : IPostRepository
    {
        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Post
            {
                PostId = x,
                Description = $"Descripción Mongo {x}",
                Date = DateTime.Now,
                Image = $"https://misapis.com/{x}",
                UserId = x * 2
            });
            await Task.Delay(10);
            return posts;
        }


        //se encarga de procesar el modelo y enviar los datos
        //Enumerable.Range(1, 10).Select genera una lista segun el modelo
        //al implemenatar una interfaz se debe cumplir con lo establecido por esa interfaz en este caso con una tarea asincorna
        /*public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Post
            {
                PostId = x,
                Description = $"Descripción {x}",
                Date = DateTime.Now,
                Image = $"https://misapis.com/{x}",
                UserId = x * 2
            });
            await Task.Delay(10);
            return posts;
        }
    }*/
}
