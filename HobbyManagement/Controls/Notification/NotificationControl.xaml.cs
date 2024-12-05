using HobbyManagement.Viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace HobbyManagement.Controls.Notification;

/// <summary>
/// Interaction logic for NotificationControl.xaml
/// </summary>
public partial class NotificationControl : UserControl
{
    public NotificationControl()
    {
        InitializeComponent();
    }

    #region DependencyProperties

    /// <summary>
    /// Dependency property controlled by property <see cref="Notifications"/>.
    /// </summary>
    public static readonly DependencyProperty NotificationsProperty =
        DependencyProperty.Register("Notifications", typeof(IList<NotificationMessage>), typeof(NotificationControl), new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>
    /// A list of notifications to show. 
    /// </summary>
    public IList<NotificationMessage> Notifications
    {
        get => (IList<NotificationMessage>)GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }

    #endregion
}
