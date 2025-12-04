using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.SaleModified;

public class SaleModifiedNotification : INotification
{
    public SaleModifiedEvent Event { get; }

    public SaleModifiedNotification(SaleModifiedEvent @event)
    {
        Event = @event;
    }
}
