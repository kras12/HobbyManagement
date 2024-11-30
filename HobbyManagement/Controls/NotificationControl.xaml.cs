using HobbyManagement.Viewmodels;
using System.Windows;
using System.Windows.Controls;

namespace HobbyManagement.Controls;

/// <summary>
/// Interaction logic for NotificationControl.xaml
/// </summary>
public partial class NotificationControl : UserControl
{
    public static readonly DependencyProperty NotificationsProperty =
        DependencyProperty.Register("Notifications", typeof(IList<NotificationMessage>), typeof(NotificationControl), new PropertyMetadata(null));

    public NotificationControl()
    {
        InitializeComponent();
    }

    public IList<NotificationMessage> Notifications
    {
        get => (IList<NotificationMessage>)GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }
}
