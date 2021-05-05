using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akkatecture.Commands;
using MediatR;
using Monads;

namespace CConv.Domain.Models.Common
{
    internal abstract class BaseQueryHandler<TQuery, TResult>
        : IRequestHandler<TQuery, IResult<TResult, string>>
        where TQuery : ICommand, IRequest<IResult<TResult, string>>
    {
        private readonly IActorRef _appActor;

        protected BaseQueryHandler( IActorRef appActor )
        {
            _appActor = appActor;
        }

        protected virtual TimeSpan GetDefaultAskTimeout() => TimeSpan.FromSeconds(5);

        public Task<IResult<TResult, string>> Handle( TQuery request, CancellationToken cancellationToken )
        {
            return _appActor.Ask<IResult<TResult, string>>(request, GetDefaultAskTimeout(), cancellationToken);
        }
    }
}