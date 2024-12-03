using HobbyManagement.Controls.Notification.ViewModels;
using HobbyManagement.Viewmodels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace HobbyManagement.Controls.HobbyList.ViewModels;

public class DesignHobbyManagerViewModel
{
    private readonly ICollectionView _hobbies;
    private readonly ObservableCollection<DesignHobbyViewModel> _hobbiesCollection;
    private readonly DesignNotificationCollectionViewModel _notificationsContext = new();
    private ObservableCollection<NotificationMessage> _notifications = new();

    public DesignHobbyManagerViewModel()
    {
        _hobbiesCollection = new()
        {
                new DesignHobbyViewModel("Hobby A", "A fine hobby."),
                new DesignHobbyViewModel("Hobby B", "It's better than nothing."),
                new DesignHobbyViewModel("Hobby C", "I have been doing it my whole life.")
        };
        _hobbies = CollectionViewSource.GetDefaultView(_hobbiesCollection);
    }

    public string GridViewSortedByColumn => nameof(IHobbyViewModel.Name);
    public ICollectionView Hobbies => _hobbies;
    public bool IsGridViewSortOrderAscending => true;
    public ObservableCollection<NotificationMessage> Notifications => _notifications;
    public DesignNotificationCollectionViewModel NotificationsContext => _notificationsContext;
}
