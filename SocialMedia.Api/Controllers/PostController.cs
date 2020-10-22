using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        //variable de lectura para referencias
        private readonly IPostRepository _postRepository;
        //constructor ctor sirve para inyectar dependecias
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        /* lo ideal es tener un método por cada httpd*/
        [HttpGet]
        public async  Task<IActionResult> GetPosts()
        { 
            //la implmentación new es de dependecia no es inyeccion de dependecias
            var posts = await _postRepository.GetPosts();
            return Ok(posts);
        }
    }
}