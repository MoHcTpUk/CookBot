using AutoMapper;
using CookBot.BLL.DTO;
using CookBot.DAL.Entities;
using Core.BLL.Services;
using Core.DAL.Repository;

namespace CookBot.BLL.Services
{
    public class PollService : AbstractService<PollEntity, PollEntityDto>
    {
        public PollService(IRepository<PollEntity> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}