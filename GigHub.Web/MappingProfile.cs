using AutoMapper;

using GigHub.Web.Models;
using GigHub.Web.Dtos;

namespace GigHub.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Gig, GigDto>();
            CreateMap<Notification, NotificationDto>();            
        }
    }
}