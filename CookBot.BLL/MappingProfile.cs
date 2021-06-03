using AutoMapper;
using CookBot.BLL.DTO;
using CookBot.DAL.Entities;

namespace CookBot.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PollEntity, PollEntityDto>();
            CreateMap<PollEntityDto, PollEntity>();
        }
    }
}