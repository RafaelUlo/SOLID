using MediatR;

namespace IPL.SOLID.API.Application.Responses;

public interface ISemanticRequestHandler<TRequest, TContent>
    : IRequestHandler<TRequest, SemanticResponse<TContent>>
    where TRequest : IRequest<SemanticResponse<TContent>>
{
    Task<SemanticResponse<TContent>> IRequestHandler<TRequest, SemanticResponse<TContent>>.Handle(
        TRequest request, CancellationToken cancellationToken)
    {
        return Handle(request, new ResponseFactory<TContent>(), cancellationToken);
    }

    Task<SemanticResponse<TContent>> Handle(
        TRequest request, ResponseFactory<TContent> response, CancellationToken cancellationToken);
}
