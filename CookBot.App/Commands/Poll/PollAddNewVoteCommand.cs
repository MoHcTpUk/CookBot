using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CookBot.App.Commands.Poll
{
    public record PollAddNewVoteCommand(PollAnswer PollAnswer) : IRequest;

    public class PollAddNewVoteCommandHandler : IRequestHandler<PollAddNewVoteCommand>
    {
        private readonly IMongdoDbService<PollEntity> _pollService;

        public PollAddNewVoteCommandHandler(IMongdoDbService<PollEntity> pollService)
        {
            _pollService = pollService;
        }
        public async Task<Unit> Handle(PollAddNewVoteCommand request, CancellationToken cancellationToken)
        {
            var poll = (await _pollService.SelectAsync(new()
            {
                x => x.PollId.Equals(request.PollAnswer.PollId)
            })).FirstOrDefault();

            if (poll == null)
                throw new Exception("Poll not found");

            poll.VotedNo ??= new();
            poll.VotedYes ??= new();

            if (request.PollAnswer.OptionIds.Length == 0)
            {
                var itemToRemove = poll.VotedNo.FirstOrDefault(_ => _.Id == request.PollAnswer.User.Id);
                poll.VotedNo.Remove(itemToRemove);

                itemToRemove = poll.VotedYes.FirstOrDefault(_ => _.Id == request.PollAnswer.User.Id);
                poll.VotedYes.Remove(itemToRemove);
            }
            else
            {
                if (request.PollAnswer.OptionIds[0] == 0)
                {
                    var item = poll.VotedYes.FirstOrDefault(_ => _.Id == request.PollAnswer.User.Id);

                    if (item != null)
                        poll.VotedYes.Remove(item);
                    else
                        poll.VotedYes.Add(new UserEntity
                        {
                            Id = request.PollAnswer.User.Id,
                            FirstName = request.PollAnswer.User.FirstName,
                            LastName = request.PollAnswer.User.FirstName,
                            UserName = request.PollAnswer.User.Username
                        });
                }

                if (request.PollAnswer.OptionIds[0] == 1)
                {
                    var item = poll.VotedNo.FirstOrDefault(_ => _.Id == request.PollAnswer.User.Id);

                    if (item != null)
                        poll.VotedNo.Remove(item);
                    else
                        poll.VotedNo.Add(new UserEntity
                        {
                            Id = request.PollAnswer.User.Id,
                            FirstName = request.PollAnswer.User.FirstName,
                            LastName = request.PollAnswer.User.FirstName,
                            UserName = request.PollAnswer.User.Username
                        });
                }
            }

            await _pollService.UpdateAsync(poll);

            return Unit.Value;
        }
    }

}
