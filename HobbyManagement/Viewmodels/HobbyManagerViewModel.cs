﻿using AutoMapper;
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

namespace HobbyManagement.Viewmodels;

public class HobbyManagerViewModel : ObservableObjectBase, IHobbyManagerViewModel
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly ObservableCollection<IHobbyViewModel> _hobbies;
    private readonly HobbyManager _hobbyManager = new HobbyManager();
    private string _gridViewSortedByColumn = "";
    private bool _gridViewSortOrderIsAscending = true;
    private ICollectionView _hobbiesView = default!;
    private bool _isLoadingData;
    private string _searchText = "";
    private readonly IHobbyViewModelFactory _hobbyViewModelFactory;

    #endregion

    #region Constructors

    public HobbyManagerViewModel(IHobbyViewModelFactory hobbyViewModelFactory, IMapper mapper)
    {
        _hobbyViewModelFactory = hobbyViewModelFactory;
        _mapper = mapper;

        _hobbies = new ObservableCollection<IHobbyViewModel>();
        HobbiesView = CollectionViewSource.GetDefaultView(_hobbies);
        HobbiesView.Filter = FilterHobbies;
        ((ListCollectionView)HobbiesView).CustomSort = Comparer<IHobbyViewModel>.Create(CompareHobbySortOrder);
        _hobbyManager.HobbiesChanged += HobbiesChangedEventHandler;
        SortGridViewByColumnCommand = new GenericRelayCommand<string>(SortHobbyList);
        AddHobbyCommand = new RelayCommand(AddEmptyHobby);
        DeleteHobbyCommand = new GenericRelayCommand<IHobbyViewModel>(DeleteHobby, CanDeleteHobby);

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
            HobbiesView.Refresh();
        }
    }

    #endregion

    #region Commands

    public ICommand AddHobbyCommand { get; }
    public ICommand DeleteHobbyCommand { get; }
    public ICommand SortGridViewByColumnCommand { get; }

    #endregion

    #region CommandMethods 

    private void AddEmptyHobby()
    {
        var newHobby = _hobbyViewModelFactory.CreateHobbyViewModel();
        newHobby.StartEdit();
        _hobbies.Add(newHobby);

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
            }
        }
    }

    private void SortHobbyList(string columnName)
    {
        GridViewSortOrderIsAscending = !GridViewSortOrderIsAscending;
        GridViewSortedByColumn = columnName;
        HobbiesView.Refresh();
    }

    #endregion

    #region Methods

    private void AddHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            _hobbies.Add(_mapper.Map<IHobbyViewModel>(hobby));
        }
    }

    private void ClearHobbies()
    {
        _hobbies.Clear();
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

    private void SetDefaultHobbyListSorting()
    {
        GridViewSortedByColumn = nameof(IHobbyViewModel.Name);
        GridViewSortOrderIsAscending = true;
        HobbiesView.Refresh();
    }

    #endregion
}
