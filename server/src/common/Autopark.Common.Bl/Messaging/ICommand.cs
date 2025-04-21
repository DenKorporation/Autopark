using FluentResults;
using MediatR;

namespace Autopark.Common.Bl.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommand : IRequest<Result>
{
}
