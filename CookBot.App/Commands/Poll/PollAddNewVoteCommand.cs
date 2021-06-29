using CookBot.DAL.Entities;
using Core.Module.MongoDb.Services;
using MediatR;
using System;
using System.Collections.Generic;
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

            poll.VotedNo ??= new List<int>();
            poll.VotedYes ??= new List<int>();

            if (request.PollAnswer.OptionIds.Length == 0)
            {
                poll.VotedNo.Remove(request.PollAnswer.User.Id);
                poll.VotedYes.Remove(request.PollAnswer.User.Id);
            }
            else
            {
                if (request.PollAnswer.OptionIds[0] == 0)
                {
                    if (poll.VotedNo.Contains(request.PollAnswer.User.Id))
                        poll.VotedNo.Remove(request.PollAnswer.User.Id);
                    else
                    {
                        poll.VotedNo.Add(request.PollAnswer.User.Id);
                    }
                }

                if (request.PollAnswer.OptionIds[0] == 1)
                {
                    if (poll.VotedYes.Contains(request.PollAnswer.User.Id))
                        poll.VotedYes.Remove(request.PollAnswer.User.Id);
                    else
                    {
                        poll.VotedYes.Add(request.PollAnswer.User.Id);
                    }
                }
            }

            await _pollService.UpdateAsync(poll);

            return Unit.Value;
        }
    }

}
