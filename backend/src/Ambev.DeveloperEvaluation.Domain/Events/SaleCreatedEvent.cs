using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent
{
    public Cart Cart { get; }

    public SaleCreatedEvent(Cart cart)
    {
        Cart = cart;
    }
}
