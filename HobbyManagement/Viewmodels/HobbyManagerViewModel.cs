using HobbyManagement.Commands;
using HobbyManagment.Data;
using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace HobbyManagement.Viewmodels;

public class HobbyManagerViewModel : ObservableObjectBase
{
    #region Fields
    
    private readonly HobbyManager _hobbyManager = new HobbyManager();
    private readonly ObservableCollection<HobbyViewModel> _hobbies;
    private bool _isLoadingData;
    private string _filterText = "";
    private ICollectionView _hobbiesView = default!;
    private string _gridViewSortedByColumn = "";
    private bool _gridViewSortOrderIsAscending = true;

    #endregion

    #region Constructors

    public HobbyManagerViewModel()
    {
        _hobbies = new ObservableCollection<HobbyViewModel>(_hobbyManager.Hobbies.Select(x => new HobbyViewModel(x)).ToList());
        HobbiesView = CollectionViewSource.GetDefaultView(_hobbies);
        HobbiesView.Filter = FilterHobbies;
        _hobbyManager.HobbiesChanged += HobbiesChangedEventHandler;
        SortGridViewByColumnCommand = new GenericRelayCommand<string>(SortHobbyList, (_) => true);
        AddHobbyCommand = new RelayCommand(AddHobby, () => true);
        DeleteHobbyCommand = new GenericRelayCommand<HobbyViewModel>(DeleteHobby, CanDeleteHobby);        

        LoadDataAsync();
    }    

    #endregion

    #region Properties

    public string FilterText
    {
        get
        {
            return _filterText;
        }

        set
        {
            _filterText = value;
            RaisePropertyChanged(nameof(FilterText));
            HobbiesView.Refresh();
        }
    }

    public string GridViewSortedByColumn
    {
        get
        {
            return _gridViewSortedByColumn;
        }

        set
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

        set
        {
            _gridViewSortOrderIsAscending = value;
            RaisePropertyChanged(nameof(GridViewSortOrderIsAscending));
        }
    }

    public ICollectionView HobbiesView
    {
        get
        {
            return _hobbiesView;
        }

        private init
        {
            _hobbiesView = value;
            RaisePropertyChanged(nameof(HobbiesView));
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

    #endregion

    #region Commands

    public RelayCommand AddHobbyCommand { get; }
    public GenericRelayCommand<HobbyViewModel> DeleteHobbyCommand { get; }
    public GenericRelayCommand<string> SortGridViewByColumnCommand { get; }

    #endregion

    #region CommandMethods 

    private void AddHobby()
    {
        var newHobby = new HobbyViewModel(new Hobby("", ""));
        newHobby.StartEdit();
        _hobbies.Add(newHobby);
        RaisePropertyChanged(nameof(HobbiesView));

        newHobby.OnCancelEditHobby += (sender, hobby) =>
        {
            _hobbies.Remove(newHobby);
        };

        newHobby.OnSaveEditHobby += (sender, hobby) =>
        {
            _hobbies.Remove(newHobby);
            _hobbyManager.AddHobby(newHobby.GetWrappedHobby());
        };
    }

    private bool CanDeleteHobby(HobbyViewModel hobby)
    {
        return hobby != null;
    }

    private void DeleteHobby(HobbyViewModel hobby)
    {
        if (CanDeleteHobby(hobby))
        {
            if (MessageBox.Show("Are you sure you want to delete this hobby?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _hobbyManager.DeleteHobby(hobby.GetWrappedHobby());
            }            
        }
    }

    private void SortHobbyList(string columnName)
    {
        HobbiesView.SortDescriptions.Clear();
        GridViewSortOrderIsAscending = !GridViewSortOrderIsAscending;
        GridViewSortedByColumn = columnName;
        HobbiesView.SortDescriptions.Add(new SortDescription(columnName, GridViewSortOrderIsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending));
        HobbiesView.Refresh();
    }

    #endregion

    #region Methods

    private void AddHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            _hobbies.Add(new HobbyViewModel(hobby));
        }
    }

    private void ClearHobbies()
    {
        _hobbies.Clear();
    }

    private bool FilterHobbies(object item)
    {
        if (item is HobbyViewModel hobby)
        {
            return string.IsNullOrEmpty(FilterText) 
                || hobby.Name.Contains(_filterText, StringComparison.CurrentCultureIgnoreCase)
                || hobby.Description.Contains(_filterText, StringComparison.CurrentCultureIgnoreCase);
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
            SetDefaultSorting();
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
        _hobbies.Move(oldStartingIndex, newStartingIndex);
    }

    private void RemoveHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            var hobbyToRemove = _hobbies.FirstOrDefault(x => x.GetWrappedHobby() == hobby);

            if (hobbyToRemove != null)
            {
                _hobbies.Remove(hobbyToRemove);
            }
        }
    }

    private void SetDefaultSorting()
    {
        GridViewSortedByColumn = nameof(HobbyViewModel.Name);
        GridViewSortOrderIsAscending = true;
        HobbiesView.SortDescriptions.Clear();
        HobbiesView.SortDescriptions.Add(new SortDescription(nameof(HobbyViewModel.Name), _gridViewSortOrderIsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending));
        HobbiesView.Refresh();
    }

    #endregion
}
