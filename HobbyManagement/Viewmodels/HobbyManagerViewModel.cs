using AutoMapper;
using HobbyManagement.Commands;
using HobbyManagement.Services;
using HobbyManagment.Data;
using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace HobbyManagement.Viewmodels;

public class HobbyManagerViewModel : ObservableObjectBase, IHobbyManagerViewModel
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly ObservableCollection<IHobbyViewModel> _hobbiesCollection;
    private readonly HobbyManager _hobbyManager = new HobbyManager();
    private string _gridViewSortedByColumn = "";
    private bool _gridViewSortOrderIsAscending = true;
    private ICollectionView _hobbies = default!;
    private bool _isLoadingData;
    private string _searchText = "";
    private ObservableCollection<NotificationMessage> _notifications = new();
    private readonly IHobbyViewModelFactory _hobbyViewModelFactory;

    #endregion

    #region Constructors

    public HobbyManagerViewModel(IHobbyViewModelFactory hobbyViewModelFactory, IMapper mapper)
    {
        _hobbyViewModelFactory = hobbyViewModelFactory;
        _mapper = mapper;

        _hobbiesCollection = new ObservableCollection<IHobbyViewModel>();
        Hobbies = CollectionViewSource.GetDefaultView(_hobbiesCollection);
        Hobbies.Filter = FilterHobbies;
        ((ListCollectionView)Hobbies).CustomSort = Comparer<IHobbyViewModel>.Create(CompareHobbySortOrder);
        _hobbyManager.HobbiesChanged += HobbiesChangedEventHandler;
        SortGridViewByColumnCommand = new GenericRelayCommand<string>(SortHobbyList);
        AddHobbyCommand = new RelayCommand(AddEmptyHobby);
        DeleteHobbyCommand = new GenericRelayCommand<IHobbyViewModel>(DeleteHobby, CanDeleteHobby);
        RemoveNotificationCommand = new GenericRelayCommand<NotificationMessage>(RemoveNotification);

        SetDefaultHobbyListSorting();
        LoadDataAsync();
        
    }

    #endregion

    #region Properties

    public string GridViewSortedByColumn
    {
        get
        {
            return _gridViewSortedByColumn;
        }

        private set
        {
            _gridViewSortedByColumn = value;
            RaisePropertyChanged(nameof(GridViewSortedByColumn));
        }
    }

    public bool GridViewSortOrderIsAscending
    {
        get
        {
            return _gridViewSortOrderIsAscending;
        }

        private set
        {
            _gridViewSortOrderIsAscending = value;
            RaisePropertyChanged(nameof(GridViewSortOrderIsAscending));
        }
    }

    public ICollectionView Hobbies
    {
        get
        {
            return _hobbies;
        }

        private init
        {
            _hobbies = value;
            RaisePropertyChanged(nameof(Hobbies));
        }
    }

    public ObservableCollection<NotificationMessage> Notifications
    {
        get
        {
            return _notifications;
        }

        set
        {
            _notifications = value;
            RaisePropertyChanged(nameof(Notifications));
        }
    }

    public bool IsLoadingData
    {
        get
        {
            return _isLoadingData;
        }

        private set
        {
            _isLoadingData = value;
            RaisePropertyChanged(nameof(IsLoadingData));
        }
    }

    public string SearchText
    {
        get
        {
            return _searchText;
        }

        set
        {
            _searchText = value;
            RaisePropertyChanged(nameof(SearchText));
            Hobbies.Refresh();
        }
    }

    #endregion

    #region Commands

    public ICommand AddHobbyCommand { get; }
    public ICommand DeleteHobbyCommand { get; }
    public ICommand RemoveNotificationCommand { get; }
    public ICommand SortGridViewByColumnCommand { get; }

    #endregion

    #region CommandMethods 

    private void AddEmptyHobby()
    {
        var newHobby = _hobbyViewModelFactory.CreateHobbyViewModel();
        newHobby.StartEdit();
        _hobbiesCollection.Add(newHobby);

        newHobby.OnCancelEditHobby += (sender, hobby) =>
        {
            _hobbies.Remove(newHobby);
        };

        newHobby.OnSaveEditHobby += (sender, hobby) =>
        {
            _hobbies.Remove(newHobby);
            _hobbyManager.AddHobby(newHobby.GetWrappedHobby());
            ShowNotification("Created hobby.");
        };
    }

    private bool CanDeleteHobby(IHobbyViewModel hobby)
    {
        return hobby != null;
    }

    private void DeleteHobby(IHobbyViewModel hobby)
    {
        if (CanDeleteHobby(hobby))
        {
            if (MessageBox.Show("Are you sure you want to delete this hobby?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _hobbyManager.DeleteHobby(hobby.GetWrappedHobby());
                ShowNotification("Deleted hobby.");
            }
        }
    }

    private void RemoveNotification(NotificationMessage notification)
    {
        _notifications.Remove(notification);
    }

    private void SortHobbyList(string columnName)
    {
        GridViewSortOrderIsAscending = !GridViewSortOrderIsAscending;
        GridViewSortedByColumn = columnName;
        Hobbies.Refresh();
    }

    #endregion

    #region Methods

    private void AddHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            var newHobbyViewModel = _mapper.Map<IHobbyViewModel>(hobby);
            newHobbyViewModel.OnSaveEditHobby += (_, _) =>
            {
                ShowNotification("Hobby updated.");
            };

            _hobbiesCollection.Add(newHobbyViewModel);
        }
    }

    private void ClearHobbies()
    {
        _hobbiesCollection.Clear();
    }

    private void ShowNotification(string message)
    {
        var notification = new NotificationMessage(message);
        _notifications.Insert(0, notification);

        var timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2) };
        timer.Tick += (sender, e) =>
        {
            _notifications.Remove(notification);
            timer.Stop();
        };
        timer.Start();
    }

    private int CompareHobbySortOrder(IHobbyViewModel hobbyA, IHobbyViewModel hobbyB)
    {
        if (hobbyA.IsEmpty() && !hobbyB.IsEmpty())
        {
            return 1;
        }
        else if (!hobbyA.IsEmpty() && hobbyB.IsEmpty())
        {
            return -1;
        }

        int result = 0;

        switch (GridViewSortedByColumn)
        {
            case nameof(IHobbyViewModel.Name):
                result = result = Comparer<string>.Default.Compare(hobbyA.Name, hobbyB.Name);
                break;

            case nameof(IHobbyViewModel.Description):
                result = Comparer<string>.Default.Compare(hobbyA.Description, hobbyB.Description);
                break;
        }

        if (result != 0)
        {
            return GridViewSortOrderIsAscending ? result : -result;
        }

        return 0;
    }

    private bool FilterHobbies(object item)
    {
        if (item is IHobbyViewModel hobby)
        {
            return string.IsNullOrEmpty(SearchText)
                || hobby.Name.Contains(_searchText, StringComparison.CurrentCultureIgnoreCase)
                || hobby.Description.Contains(_searchText, StringComparison.CurrentCultureIgnoreCase);
        }

        return false;
    }

    private void HobbiesChangedEventHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddHobbies(e.NewItems!.Cast<Hobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Remove:
                RemoveHobbies(e.OldItems!.Cast<Hobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Reset:
                ClearHobbies();
                AddHobbies(_hobbyManager.Hobbies.ToList());
                break;

            case NotifyCollectionChangedAction.Replace:
                RemoveHobbies(e.OldItems!.Cast<Hobby>().ToList());
                AddHobbies(e.NewItems!.Cast<Hobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Move:
                MoveHobby(e.OldStartingIndex, e.NewStartingIndex);
                break;
        }
    }

    private async void LoadDataAsync()
    {
        try
        {
            IsLoadingData = true;
            await _hobbyManager.LoadData();
        }
        catch (Exception ex)
        {
            // TOOD
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsLoadingData = false;
        }
    }

    private void MoveHobby(int oldStartingIndex, int newStartingIndex)
    {
        _hobbiesCollection.Move(oldStartingIndex, newStartingIndex);
    }

    private void RemoveHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            var hobbyToRemove = _hobbiesCollection.FirstOrDefault(x => x.GetWrappedHobby() == hobby);

            if (hobbyToRemove != null)
            {
                _hobbiesCollection.Remove(hobbyToRemove);
            }
        }
    }

    private void SetDefaultHobbyListSorting()
    {
        GridViewSortedByColumn = nameof(IHobbyViewModel.Name);
        GridViewSortOrderIsAscending = true;
        Hobbies.Refresh();
    }

    #endregion
}
