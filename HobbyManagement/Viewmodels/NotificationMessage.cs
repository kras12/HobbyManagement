namespace HobbyManagement.Viewmodels;

public class NotificationMessage : INotificationMessage
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">The message of the notification.</param>
    public NotificationMessage(string message)
    {
        Message = message;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The message of the notification.
    /// </summary>
    public string Message { get; set; } = "";

    #endregion
}
