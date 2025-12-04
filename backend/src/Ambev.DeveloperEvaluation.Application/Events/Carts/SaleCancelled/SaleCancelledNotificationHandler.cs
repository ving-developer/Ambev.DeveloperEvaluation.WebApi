using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.SaleCancelled;

public class SaleCancelledNotificationHandler : INotificationHandler<SaleCancelledNotification>
{
    private readonly ILogger<SaleCancelledNotificationHandler> _logger;

    public SaleCancelledNotificationHandler(ILogger<SaleCancelledNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "================Notifying user {CustomerId} about the cancellation of Cart {CartId} | Cancellation Reason: {Reason}================",
            notification.Event.Cart.CustomerId,
            notification.Event.Cart.Id,
            notification.Event.Reason
        );

        return Task.CompletedTask;
    }
}
