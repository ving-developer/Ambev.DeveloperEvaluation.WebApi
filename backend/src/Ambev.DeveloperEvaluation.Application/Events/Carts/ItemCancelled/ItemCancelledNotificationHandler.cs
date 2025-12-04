using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.ItemCancelled;

public class ItemCancelledNotificationHandler : INotificationHandler<ItemCancelledNotification>
{
    private readonly ILogger<ItemCancelledNotificationHandler> _logger;

    public ItemCancelledNotificationHandler(ILogger<ItemCancelledNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ItemCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "================Notifying user {CustomerId} about the cancellation of Item {Title} from Cart {CartId}================",
            notification.Event.Cart.CustomerId,
            notification.Event.Item.Product.Title,
            notification.Event.Cart.Id
        );

        return Task.CompletedTask;
    }
}
