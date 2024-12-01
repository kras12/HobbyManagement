using HobbyManagement.Viewmodels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HobbyManagement.Controls.HobbyList;

/// <summary>
/// Interaction logic for HobbyListControl.xaml
/// </summary>
public partial class HobbyListControl : UserControl
{
    public HobbyListControl()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty AddHobbyCommandProperty =
        DependencyProperty.Register("AddHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty CancelEditHobbyCommandProperty =
        DependencyProperty.Register("CancelEditHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty DeleteHobbyCommandProperty =
        DependencyProperty.Register("DeleteHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty GridViewSortedByColumnProperty =
        DependencyProperty.Register("GridViewSortedByColumn", typeof(string), typeof(HobbyListControl), new PropertyMetadata(""));

    public static readonly DependencyProperty HobbiesProperty =
        DependencyProperty.Register("Hobbies", typeof(IList<IHobbyViewModel>), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty IsGridViewSortOrderAscendingProperty =
        DependencyProperty.Register("IsGridViewSortOrderAscending", typeof(bool), typeof(HobbyListControl), new PropertyMetadata(false));

    public static readonly DependencyProperty NotificationsProperty =
        DependencyProperty.Register("Notifications", typeof(IList<INotificationMessage>), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty RemoveNotificationCommandProperty =
        DependencyProperty.Register("RemoveNotificationCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty SaveHobbyCommandProperty =
        DependencyProperty.Register("SaveHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty SortGridViewByColumnCommandProperty =
                            DependencyProperty.Register("SortGridViewByColumnCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public static readonly DependencyProperty StartEditHobbyCommandProperty =
       DependencyProperty.Register("StartEditHobbyCommand", typeof(ICommand), typeof(HobbyListControl), new PropertyMetadata(null));

    public ICommand AddHobbyCommand
    {
        get => (ICommand)GetValue(AddHobbyCommandProperty);
        set => SetValue(AddHobbyCommandProperty, value);
    }

    public ICommand CancelEditHobbyCommand
    {
        get => (ICommand)GetValue(CancelEditHobbyCommandProperty);
        set => SetValue(CancelEditHobbyCommandProperty, value);
    }

    public ICommand DeleteHobbyCommand
    {
        get => (ICommand)GetValue(DeleteHobbyCommandProperty);
        set => SetValue(DeleteHobbyCommandProperty, value);
    }

    public string GridViewSortedByColumn
    {
        get => (string)GetValue(GridViewSortedByColumnProperty);
        set => SetValue(GridViewSortedByColumnProperty, value);
    }

    public IList<IHobbyViewModel> Hobbies
    {
        get => (IList<IHobbyViewModel>)GetValue(HobbiesProperty);
        set => SetValue(HobbiesProperty, value);
    }

    public bool IsGridViewSortOrderAscending
    {
        get => (bool)GetValue(IsGridViewSortOrderAscendingProperty);
        set => SetValue(IsGridViewSortOrderAscendingProperty, value);
    }

    public IList<INotificationMessage> Notifications
    {
        get => (IList<INotificationMessage>)GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }

    public ICommand RemoveNotificationCommand
    {
        get => (ICommand)GetValue(RemoveNotificationCommandProperty);
        set => SetValue(RemoveNotificationCommandProperty, value);
    }

    public ICommand SaveHobbyCommand
    {
        get => (ICommand)GetValue(SaveHobbyCommandProperty);
        set => SetValue(SaveHobbyCommandProperty, value);
    }

    public ICommand SortGridViewByColumnCommand
    {
        get => (ICommand)GetValue(SortGridViewByColumnCommandProperty);
        set => SetValue(SortGridViewByColumnCommandProperty, value);
    }

    public ICommand StartEditHobbyCommand
    {
        get => (ICommand)GetValue(StartEditHobbyCommandProperty);
        set => SetValue(StartEditHobbyCommandProperty, value);
    }
}
