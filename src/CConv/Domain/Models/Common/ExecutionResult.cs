using Monads;

namespace CConv.Domain.Models.Common
{
    public static class ExecutionResult
    {
        public static IResult<Nothing, string> Failure( string failure ) =>
            new FailureResult<Nothing, string>(failure);

        public static IResult<Nothing,string> Success() => new SuccessResult<Nothing, string>(Nothing.Instance);
    }
}