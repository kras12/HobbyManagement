using System.Collections.ObjectModel;

namespace HobbyManagement.Controls.Notification.ViewModels;

/// <summary>
/// Design time view model that contains a collection of notification messages. 
/// </summary>
public class DesignNotificationCollectionViewModel
{
    #region Fields

    /// <summary>
    /// Backing field for property <see cref="Notifications"/>.
    /// </summary>
    private readonly ObservableCollection<DesignNotificationMessage> _notifications = new()
    {
        new DesignNotificationMessage() {Message = "An item was added."},
        new DesignNotificationMessage() {Message = "An item was deleted."}
    };

    #endregion

    #region Properties

    /// <summary>
    /// An observable collection that contains notification messages. 
    /// </summary>
    public ObservableCollection<DesignNotificationMessage> Notifications => _notifications;

    #endregion
}
