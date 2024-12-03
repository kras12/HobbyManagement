using System.Collections.ObjectModel;

namespace HobbyManagement.Controls.Notification.ViewModels;

public class DesignNotificationCollectionViewModel
{
    private readonly ObservableCollection<DesignNotificationMessage> _notifications = new()
    {
        new DesignNotificationMessage() {Message = "An item was added."},
        new DesignNotificationMessage() {Message = "An item was deleted."}
    };

    public ObservableCollection<DesignNotificationMessage> Notifications => _notifications;
}
