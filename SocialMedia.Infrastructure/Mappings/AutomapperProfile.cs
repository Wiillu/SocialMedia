using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //si la entidad dto es diferente a los del la entidad es mejor no usarala el map
            // Se mapea los tipos de datos con automapper de .net core
            CreateMap < Post, PostDto > ();
            CreateMap<PostDto, Post>();
        }
    }
}
