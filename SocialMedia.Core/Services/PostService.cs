using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        //con ctrl + r + i se genera una nueva Interfaz según la clase

        //private readonly IPostRepository _postRepository; al tener declarada el repositorio genrico se dbe inyectar la interfaz generica que pase la entidad
        //private readonly IUserRepository _userRepository; // declaramos e inyectamos

        /*private readonly IRepository<Post> _postRepository;//  !!!este patron no es obligatorio!!!
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Comment> _commentRepository;*/

        private readonly IUnitOfWork _unitOfWork;

        //reglas de negocios
        //public PostService(IPostRepository postRepository, IUserRepository userRepository)

        //public PostService(IRepository<Post> postRepository, IRepository<User> userRepository)//cuando se tiene el repositorio generico se deben declarar la interfaz generica con su entidad

        public PostService(IUnitOfWork unitOfWork)//Llamamos donde se unificaron los repositorios
        {//inyectamos las interfaces.
            /*_postRepository = postRepository;
            _userRepository = userRepository;*/
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeletePost(int id)
        {
             await _unitOfWork.PostRepository.Delete(id);//usamos los metodos declarado en la interfaz generica
            return true;//nos retorna un valor bool 
        }

        public async Task<Post> GetPost(int id)
        {
            //return await _postRepository.GetById(id);
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public PagedList<Post> GetPosts(PostQueryFilters filters)
        {//uqery filter
            //return await _postRepository.GetPosts(); se cambia por la del repositorio generico
            //return await _unitOfWork.PostRepository.GetAll();
            var posts = _unitOfWork.PostRepository.GetAll();
            if(filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }
            if (filters.Date != null)
            {//ToShortDateString toma un formato de fecha sin horas
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);//paginación
            return pagedPosts;
        }

        public async Task InsertPost(Post post)
        {
            //validar usuario que viene desde el repositorio

            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if(user == null)
            {
                throw new BussinessException("User doesn't exist");
            }
            if (post.Description.Contains("Sexo"))
            {
                throw new BussinessException("Content not allowed");
            }
            var userpost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if(userpost.Count() < 10)
            {
                //var lastPost = userpost.OrderBy(x => x.Date);
                //var lastPost = userpost.LastOrDefault();
                var lastPost = userpost.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BussinessException("You Are not able to publish the post");
                }
            }
            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
             _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
