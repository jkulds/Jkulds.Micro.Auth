using System.Text.Json;
using Jkulds.Micro.MessageContracts.Notifications;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Jkulds.Micro.Auth.Business.Producers;

public class NotificationProducer
{
    private readonly ILogger<NotificationProducer> _logger;
    private readonly IBus _bus;

    public NotificationProducer(ILogger<NotificationProducer> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task SendNotification(NotificationToSend notification)
    {
        try
        {
            await _bus.Publish(notification);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "error on message publish, notificaton = " + JsonSerializer.Serialize(notification));
            throw;
        }
    }
}