using FluentResults;
using MediatR;

namespace Autopark.Common.Bl.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
