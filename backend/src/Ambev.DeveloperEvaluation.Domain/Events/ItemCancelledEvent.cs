using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Evento disparado quando um item do carrinho é cancelado
/// </summary>
public class ItemCancelledEvent
{
    public Cart Cart { get; }
    public CartItem Item { get; }

    public ItemCancelledEvent(Cart cart, CartItem item)
    {
        Cart = cart;
        Item = item;
    }
}
