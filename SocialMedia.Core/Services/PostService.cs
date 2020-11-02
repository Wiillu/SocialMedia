using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        //con ctrl + r + i se genera una nueva Interfaz según la clase

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository; // declaramos e inyectamos

        //reglas de negocios
        public PostService(IPostRepository postRepository, IUserRepository userRepository)
        {//inyectamos las interfaces.
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> DeletePost(int id)
        {
            return await _postRepository.DeletePost(id);
        }

        public async Task<Post> GetPost(int id)
        {
            return await _postRepository.GetPost(id);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postRepository.GetPosts();
        }

        public async Task InsertPost(Post post)
        {
            //validar usuario que viene desde el repositorio

            var user = await _userRepository.GetUser(post.UserId);
            if(user == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new Exception("Content not allowed");
            }
            await _postRepository.InsertPost(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            return await _postRepository.UpdatePost(post);
        }
    }
}
