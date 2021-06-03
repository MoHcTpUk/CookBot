using System.Linq;
using CookBot.BLL.DTO;
using Core.BLL.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CookBot.DAL.Entities;

namespace CookBot.App.Commands
{
    public class CreatePoolRequest : IRequest<PollEntityDto>
    {

    }

    public class ExsampleRequestHandler : IRequestHandler<CreatePoolRequest, PollEntityDto>
    {
        private readonly IService<PollEntity, PollEntityDto> _service;

        public ExsampleRequestHandler(IService<PollEntity, PollEntityDto> service)
        {
            _service = service;
        }

        public async Task<PollEntityDto> Handle(CreatePoolRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var dto = _service.Select(a => !a.isDeleted).FirstOrDefault();

                return dto;
            }, cancellationToken);
        }
    }
}