using Jkulds.Micro.Base.Enum;

namespace Jkulds.Micro.Base.Dto.Notifications;

public class NotificationBaseDto : DtoBase
{
    public ENotificationStatus Status { get; set; }
    public ENotificationType Type { get; set; }
    public DateTime SentAt { get; set; }
    public string Body { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
}