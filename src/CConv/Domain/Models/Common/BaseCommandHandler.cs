using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akkatecture.Aggregates;
using Akkatecture.Commands;
using Akkatecture.Core;
using MediatR;
using Monads;

namespace CConv.Domain.Models.Common
{
    internal abstract class BaseCommandHandler<TAggregate, TAggregateId, TCommand>
        : IRequestHandler<TCommand, IResult<Nothing, string>>
        where TAggregate : IAggregateRoot<TAggregateId>
        where TAggregateId : Identity<TAggregateId>
        where TCommand : ICommand<TAggregate, TAggregateId>, IRequest<IResult<Nothing, string>>
    {
        private readonly IActorRef _appActor;

        protected BaseCommandHandler( IActorRef appActor )
        {
            _appActor = appActor;
        }

        Task<IResult<Nothing, string>> IRequestHandler<TCommand, IResult<Nothing, string>>.Handle
        (
            TCommand          command,
            CancellationToken cancellationToken
        )
        {
            return _appActor.Ask<IResult<Nothing, string>>(command, GetDefaultAskTimeout(), cancellationToken);
        }

        protected virtual TimeSpan GetDefaultAskTimeout() => TimeSpan.FromSeconds(5);
    }
}