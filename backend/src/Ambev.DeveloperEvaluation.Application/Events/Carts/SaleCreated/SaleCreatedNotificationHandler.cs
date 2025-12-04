using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.SaleCreated;

public class SaleCreatedNotificationHandler : INotificationHandler<SaleCreatedNotification>
{
    private readonly ILogger<SaleCreatedNotificationHandler> _logger;

    public SaleCreatedNotificationHandler(ILogger<SaleCreatedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "================Notifying user {CustomerId} by email about the purchase of Cart {CartId}================",
            notification.Event.Cart.CustomerId,
            notification.Event.Cart.Id
        );

        return Task.CompletedTask;
    }
}
