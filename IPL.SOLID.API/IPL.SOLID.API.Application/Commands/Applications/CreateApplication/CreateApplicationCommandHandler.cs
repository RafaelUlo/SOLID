using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Dtos.Applications;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Domain.Exceptions;
using IPL.SOLID.API.Domain.Factories;
using IPL.SOLID.API.Domain.Policies;
using IPL.SOLID.API.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Commands.Applications.CreateApplication;

public sealed class CreateApplicationCommandHandler : ISemanticRequestHandler<CreateApplicationCommand, ApplicationDto>
{
    private readonly ISolidWriteContext _writeContext;
    private readonly ISolidReadContext _readContext;
    private readonly ICreateApplicationPolicy _createApplicationPolicy;

    public CreateApplicationCommandHandler(
        ISolidWriteContext writeContext,
        ISolidReadContext readContext,
        ICreateApplicationPolicy createApplicationPolicy)
    {
        _writeContext = writeContext;
        _readContext = readContext;
        _createApplicationPolicy = createApplicationPolicy;
    }

    public async Task<SemanticResponse<ApplicationDto>> Handle(
        CreateApplicationCommand request, ResponseFactory<ApplicationDto> response, CancellationToken cancellationToken)
    {
        var resourceName = string.IsNullOrWhiteSpace(request.ResourceName) ? Guid.NewGuid().ToString("N") : request.ResourceName;
        var application = await ApplicationFactory.CreateApplicationAsync(resourceName, _createApplicationPolicy, cancellationToken);

        application.Description = request.Description;

        try
        {
            await _writeContext.Applications.AddAsync(application, cancellationToken);
            await _writeContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            var applicationExists = await _readContext.Applications
                .AnyAsync(a => a.ResourceName == resourceName, cancellationToken);

            if (applicationExists)
            {
                throw new DomainException($"An application with the resource name '{resourceName}' already exists.");
            }

            throw;
        }

        return response.Created(ApplicationMapping.CreateApplicationDto(application));
    }
}
