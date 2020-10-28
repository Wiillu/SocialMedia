using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PostController : ControllerBase
    {
        //variable de lectura para referencias
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper; //inyecta el mapper
        //constructor ctor sirve para inyectar dependecias
        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        /* lo ideal es tener un método por cada httpd*/
        [HttpGet]
        public async  Task<IActionResult> GetPosts()
        { //solo manda una lista
            //la implmentación new es de dependecia no es inyeccion de dependecias
            
            var posts = await _postRepository.GetPosts();
            var postDto = _mapper.Map<IEnumerable<PostDto>>(posts);
                /*posts.Select(x => new PostDto
            { 
                PostId = x.PostId,
                Date = x.Date,
                Description = x.Description,
                Image = x.Image,
                UserId = x.UserId
            });*/
            return Ok(postDto);
            //BadRequest()
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            //la implmentación new es de dependecia no es inyeccion de dependecias
            var post = await _postRepository.GetPost(id);
            var postDto = _mapper.Map<PostDto>(post);
            /*new PostDto
        {
            PostId = post.PostId,
            Date = post.Date,
            Description = post.Description,
            Image = post.Image,
            UserId = post.UserId
        };*/
            return Ok(postDto);
        }

        //añadir post
        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            /*validacion manual
             * if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            //la implmentación new es de dependecia no es inyeccion de dependecias
            var post = _mapper.Map<Post>(postDto);//mapeando
            /*new Post
        {
            //pasamos la estructura DTO a la de post
            Date = postDto.Date,
            Description = postDto.Description,
            Image = postDto.Image,
            UserId = postDto.UserId
        };*/
            await _postRepository.InsertPost(post);
            return Ok(post);
        }
    }
}