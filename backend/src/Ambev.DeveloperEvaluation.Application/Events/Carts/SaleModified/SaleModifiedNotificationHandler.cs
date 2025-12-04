using Ambev.DeveloperEvaluation.Application.Events.Carts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Events.Carts.SaleModified;

public class SaleModifiedNotificationHandler : INotificationHandler<SaleModifiedNotification>
{
    private readonly ILogger<SaleModifiedNotificationHandler> _logger;

    public SaleModifiedNotificationHandler(ILogger<SaleModifiedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "================Notify user {CustomerId} by email about the update to the purchase of item {CartId} in their cart.================",
            notification.Event.Cart.CustomerId,
            notification.Event.Cart.Id
        );

        return Task.CompletedTask;
    }
}
