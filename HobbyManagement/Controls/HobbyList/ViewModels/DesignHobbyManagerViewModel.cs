using HobbyManagement.Controls.Notification.ViewModels;
using HobbyManagement.Viewmodels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace HobbyManagement.Controls.HobbyList.ViewModels;

/// <summary>
/// Design time viewmodel for a hobby manager. 
/// </summary>
public class DesignHobbyManagerViewModel
{
    #region Fields

    /// <summary>
    /// Backing field for property <see cref="Hobbies"/>.
    /// </summary>
    private readonly ICollectionView _hobbies;

    /// <summary>
    /// An observable collection that contains the data for the <see cref="_hobbies"/> view.
    /// </summary>
    private readonly ObservableCollection<DesignHobbyViewModel> _hobbiesCollection;

    /// <summary>
    /// Backing field for property <see cref="NotificationsContext"/>.
    /// </summary>
    private readonly DesignNotificationCollectionViewModel _notificationsContext = new();

    /// <summary>
    /// Backing field for property <see cref="Notifications"/>.
    /// </summary>
    private ObservableCollection<NotificationMessage> _notifications = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
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

    #endregion

    #region Properties
    
    /// <summary>
    /// The column to use when sorting the grid view.
    /// </summary>
    public string GridViewSortedByColumn => nameof(IHobbyViewModel.Name);

    /// <summary>
    /// Collection view for hobbies. 
    /// </summary>
    public ICollectionView Hobbies => _hobbies;

    /// <summary>
    /// Returns true if the grid view sort order is ascending. 
    /// </summary>
    public bool IsGridViewSortOrderAscending => true;

    /// <summary>
    /// An observable collection that contains the notification to show. 
    /// </summary>
    public ObservableCollection<NotificationMessage> Notifications => _notifications;

    /// <summary>
    /// The context to use for the notification messages. 
    /// </summary>
    public DesignNotificationCollectionViewModel NotificationsContext => _notificationsContext;

    #endregion
}
