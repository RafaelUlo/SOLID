using IPL.SOLID.API.Domain.DomainEvents;
using MediatR;

namespace IPL.SOLID.API.Application.DomainEvents;

public sealed record DomainEventNotification(IDomainEvent Event) : INotification;
