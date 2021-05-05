using System;

namespace Monads
{
    public interface IResult
    {
        bool IsSuccess();
    }

    public interface IResult<out TSuccess, out TFailure> : IResult
    {
        T Fold<T>( Func<TSuccess, T> onSuccess, Func<TFailure, T> onFailure );

        void Fold( Action<TSuccess> onSuccess, Action<TFailure> onFailure );

        bool IResult.IsSuccess() => Fold(_ => true, _ => false);
    }

    public sealed record SuccessResult<TSuccess, TFailure>( TSuccess Value ) : IResult<TSuccess, TFailure>
    {
        public T Fold<T>( Func<TSuccess, T> onSuccess, Func<TFailure, T> onFailure ) => onSuccess(Value);

        public void Fold( Action<TSuccess> onSuccess, Action<TFailure> onFailure )
        {
            onSuccess(Value);
        }
    }

    public sealed record FailureResult<TSuccess, TFailure>( TFailure Value ) : IResult<TSuccess, TFailure>
    {
        public T Fold<T>( Func<TSuccess, T> onSuccess, Func<TFailure, T> onFailure ) => onFailure(Value);

        public void Fold( Action<TSuccess> onSuccess, Action<TFailure> onFailure )
        {
            onFailure(Value);
        }
    }

    public static class Result
    {
        public static IResult<TS, TF> Validate<TS, TF>( this TS ts, Func<TS, bool> predicate, TF failure ) =>
            predicate(ts) ? new SuccessResult<TS, TF>(ts) : new FailureResult<TS, TF>(failure);

        public static IResult<TB, TF> Select<TA, TB, TF>( this IResult<TA, TF> result, Func<TA, TB> map ) =>
            result.Fold(ta => (IResult<TB, TF>) new SuccessResult<TB, TF>(map(ta)), tf => new FailureResult<TB, TF>(tf));

        public static IResult<TB, TF> SelectMany<TA, TB, TF>( this IResult<TA, TF> result, Func<TA, IResult<TB, TF>> bind ) =>
            result.Fold(bind, tf => new FailureResult<TB, TF>(tf));

        public static IResult<TC, TF> SelectMany<TA, TB, TC, TF>
        (
            this IResult<TA, TF>      result,
            Func<TA, IResult<TB, TF>> bind,
            Func<TA, TB, TC>          collect
        ) => result.SelectMany(bind)
                   .Select(tb => result.Fold(ta => collect(ta, tb), _ => throw new InvalidOperationException()));
    }
}