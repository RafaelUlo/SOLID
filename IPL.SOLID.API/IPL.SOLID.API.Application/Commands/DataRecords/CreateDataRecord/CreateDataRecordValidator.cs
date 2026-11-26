using FluentValidation;

namespace IPL.SOLID.API.Application.Commands.DataRecords.CreateDataRecord;

public sealed class CreateDataRecordValidator : AbstractValidator<CreateDataRecordCommand>
{
    public CreateDataRecordValidator()
    {
        RuleFor(command => command.ResourceType).Equal("data-record").OverridePropertyName("res-type");
        RuleFor(request => request.ContentType)
            .Must(type => type is "text/plain" or "application/json" or "application/xml")
            .WithMessage("A supported content-type is required.").OverridePropertyName("content-type");
        RuleFor(request => request.Content).NotEmpty().OverridePropertyName("content");
    }
}
