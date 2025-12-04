using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.SaleCreated;

public class SaleCreatedNotification : INotification
{
    public SaleCreatedEvent Event { get; }

    public SaleCreatedNotification(SaleCreatedEvent @event)
    {
        Event = @event;
    }
}
