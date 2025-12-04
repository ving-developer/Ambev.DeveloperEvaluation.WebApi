using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Evento disparado quando uma venda (carrinho) é cancelada
/// </summary>
public class SaleCancelledEvent
{
    public Cart Cart { get; }
    public string Reason { get; }

    public SaleCancelledEvent(Cart cart, string reason)
    {
        Cart = cart;
        Reason = reason;
    }
}
