using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.ItemCancelled;

public class ItemCancelledNotification : INotification
{
    public ItemCancelledEvent Event { get; }

    public ItemCancelledNotification(ItemCancelledEvent @event)
    {
        Event = @event;
    }
}
