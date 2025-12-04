using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.SaleCancelled;

public class SaleCancelledNotification : INotification
{
    public SaleCancelledEvent Event { get; }

    public SaleCancelledNotification(SaleCancelledEvent @event)
    {
        Event = @event;
    }
}
