using HobbyManagement.Viewmodels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HobbyManagement.Controls.HobbyList;

/// <summary>
/// Interaction logic for HobbyListControl.xaml
/// </summary>
public partial class HobbyListControl : UserControl
{
    #region Constructors
    
    public HobbyListControl()
    {
        InitializeComponent();
    }

    #endregion

    #region DependencyProperties

    /// <summary>
    /// Dependency property controlled by property <see cref="AddHobbyCommand"/>.
    /// </summary>
    public static readonly DependencyProperty AddHobbyCommandProperty =
        DependencyProperty.Register("AddHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="CancelEditHobbyCommand"/>.
    /// </summary>
    public static readonly DependencyProperty CancelEditHobbyCommandProperty =
        DependencyProperty.Register("CancelEditHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="DeleteHobbyCommand"/>.
    /// </summary>
    public static readonly DependencyProperty DeleteHobbyCommandProperty =
        DependencyProperty.Register("DeleteHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="GridViewSortedByColumn"/>.
    /// </summary>
    public static readonly DependencyProperty GridViewSortedByColumnProperty =
        DependencyProperty.Register("GridViewSortedByColumn", typeof(string), typeof(HobbyListControl), new PropertyMetadata(""));

    /// <summary>
    /// Dependency property controlled by property <see cref="Hobbies"/>.
    /// </summary>
    public static readonly DependencyProperty HobbiesProperty =
        DependencyProperty.Register("Hobbies", typeof(IList<IHobbyViewModel>), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="IsGridViewSortOrderAscending"/>.
    /// </summary>
    public static readonly DependencyProperty IsGridViewSortOrderAscendingProperty =
        DependencyProperty.Register("IsGridViewSortOrderAscending", typeof(bool), typeof(HobbyListControl), new PropertyMetadata(false));

    /// <summary>
    /// Dependency property controlled by property <see cref="Notifications"/>.
    /// </summary>
    public static readonly DependencyProperty NotificationsProperty =
        DependencyProperty.Register("Notifications", typeof(IList<INotificationMessage>), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="RemoveNotificationCommand"/>.
    /// </summary>
    public static readonly DependencyProperty RemoveNotificationCommandProperty =
        DependencyProperty.Register("RemoveNotificationCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="SaveHobbyCommand"/>.
    /// </summary>
    public static readonly DependencyProperty SaveHobbyCommandProperty =
        DependencyProperty.Register("SaveHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="SortGridViewByColumnCommand"/>.
    /// </summary>
    public static readonly DependencyProperty SortGridViewByColumnCommandProperty =
        DependencyProperty.Register("SortGridViewByColumnCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property controlled by property <see cref="StartEditHobbyCommand"/>.
    /// </summary>
    public static readonly DependencyProperty StartEditHobbyCommandProperty =
       DependencyProperty.Register("StartEditHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>
    /// Command to add a hobby.
    /// </summary>
    public ICommand AddHobbyCommand
    {
        get => (ICommand)GetValue(AddHobbyCommandProperty);
        set => SetValue(AddHobbyCommandProperty, value);
    }

    /// <summary>
    /// Command to cancel editing of a hobby.
    /// </summary>
    public ICommand CancelEditHobbyCommand
    {
        get => (ICommand)GetValue(CancelEditHobbyCommandProperty);
        set => SetValue(CancelEditHobbyCommandProperty, value);
    }

    /// <summary>
    /// Command to delete a hobby.
    /// </summary>
    public ICommand DeleteHobbyCommand
    {
        get => (ICommand)GetValue(DeleteHobbyCommandProperty);
        set => SetValue(DeleteHobbyCommandProperty, value);
    }

    /// <summary>
    /// Contains the name of the column the grid view is sorted by. 
    /// </summary>
    public string GridViewSortedByColumn
    {
        get => (string)GetValue(GridViewSortedByColumnProperty);
        set => SetValue(GridViewSortedByColumnProperty, value);
    }

    /// <summary>
    /// A collection of hobbies to show in the grid view. 
    /// </summary>
    public IList<IHobbyViewModel> Hobbies
    {
        get => (IList<IHobbyViewModel>)GetValue(HobbiesProperty);
        set => SetValue(HobbiesProperty, value);
    }

    /// <summary>
    /// Returns true if the grid view sort order is ascending. 
    /// </summary>
    public bool IsGridViewSortOrderAscending
    {
        get => (bool)GetValue(IsGridViewSortOrderAscendingProperty);
        set => SetValue(IsGridViewSortOrderAscendingProperty, value);
    }

    /// <summary>
    /// A collection of notifications to show. 
    /// </summary>
    public IList<INotificationMessage> Notifications
    {
        get => (IList<INotificationMessage>)GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }

    /// <summary>
    /// Command to hide a notification message. 
    /// </summary>
    public ICommand RemoveNotificationCommand
    {
        get => (ICommand)GetValue(RemoveNotificationCommandProperty);
        set => SetValue(RemoveNotificationCommandProperty, value);
    }

    /// <summary>
    /// Command to save a hobby. 
    /// </summary>
    public ICommand SaveHobbyCommand
    {
        get => (ICommand)GetValue(SaveHobbyCommandProperty);
        set => SetValue(SaveHobbyCommandProperty, value);
    }

    /// <summary>
    /// Command to toggle sort order on grid view column. 
    /// </summary>
    public ICommand SortGridViewByColumnCommand
    {
        get => (ICommand)GetValue(SortGridViewByColumnCommandProperty);
        set => SetValue(SortGridViewByColumnCommandProperty, value);
    }

    /// <summary>
    /// Command to start editing a hobby. 
    /// </summary>
    public ICommand StartEditHobbyCommand
    {
        get => (ICommand)GetValue(StartEditHobbyCommandProperty);
        set => SetValue(StartEditHobbyCommandProperty, value);
    }

    #endregion
}
