using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Response;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostController : ControllerBase
    {
        //variable de lectura para referencias
        private readonly IPostService _postService;
        private readonly IMapper _mapper; //inyecta el mapper
        private readonly IUriService _uriService;
 
        //constructor ctor sirve para inyectar dependecias
        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }
        /* lo ideal es tener un método por cada httpd*/
        [HttpGet(Name = nameof(GetPosts))]//FromQuery ayuda a pasar los paramteros
        [ProducesResponseType((int)HttpStatusCode.OK, Type =typeof(ApiResponse<IEnumerable<PostDto>>))]//tipo de dato que envía se llama matricular para la documentación
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        public IActionResult GetPosts([FromQuery]PostQueryFilters filters) //tipo de respuest interfaz generica
        //public ApiResponse<IEnumerable<PostDto>> GetPosts([FromQuery]PostQueryFilters filters)//es bueno para solo mostrar información
        { //solo manda una lista
          //la implmentación new es de dependecia no es inyeccion de dependecias

            var posts =  _postService.GetPosts(filters);
            var postDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            //<PagedList<PostDto>>(posts);
            //<IEnumerable<PostDto>>(posts);
            /*posts.Select(x => new PostDto
        { 
            PostId = x.PostId,
            Date = x.Date,
            Description = x.Description,
            Image = x.Image,
            UserId = x.UserId
        });*/


            //<PagedList<PostDto>>(postDto);
            //<IEnumerable<PostDto>>(postDto);
            //var metadata = new//objeto de tipo anonimo 
            var metadata = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.TotalPage,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
                NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
                PreviousPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString()
            };

            var response = new ApiResponse<IEnumerable<PostDto>>(postDto)
            { 
                Meta = metadata
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
            //BadRequest()
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            //la implmentación new es de dependecia no es inyeccion de dependecias
            var post = await _postService.GetPost(id);

            var postDto = _mapper.Map<PostDto>(post);

            
            /*new PostDto
        {
            PostId = post.PostId,
            Date = post.Date,
            Description = post.Description,
            Image = post.Image,
            UserId = post.UserId
        };*/

            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
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
            //var response = new ApiResponse<PostDto>(postDto);
            await _postService.InsertPost(post);

            postDto = _mapper.Map<PostDto>(post);//cuando se inserta algo solo devolver la información completa

            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }
        [HttpPut]
        public async Task<ActionResult> Put(int id, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = id;
            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
           var result = await _postService.DeletePost(id);
            //añadimos el tipo de respuesta refenrenciando ApiResponse
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

    }
}