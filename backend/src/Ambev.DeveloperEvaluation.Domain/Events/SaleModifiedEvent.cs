using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent
{
    public Cart Cart { get; }

    public SaleModifiedEvent(Cart cart)
    {
        Cart = cart;
    }
}
