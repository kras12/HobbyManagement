using AutoMapper;
using HobbyManagement.Business;
using HobbyManagement.Commands;
using HobbyManagement.Services;
using HobbyManagement.Services.Csv;
using HobbyManagement.Services.Csv.Error;
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

    private readonly ICsvService _csvService;
    private readonly ObservableCollection<IHobbyViewModel> _hobbiesCollection;
    private readonly IHobbyManager _hobbyManager;
    private readonly IHobbyViewModelFactory _hobbyViewModelFactory;
    private readonly IMapper _mapper;
    private string _gridViewSortedByColumn = "";
    private bool _gridViewSortOrderIsAscending = true;
    private ICollectionView _hobbies = default!;
    private bool _isLoadingData;
    private ObservableCollection<NotificationMessage> _notifications = new();
    private string _searchText = "";
    #endregion

    #region Constructors

    public HobbyManagerViewModel(IHobbyViewModelFactory hobbyViewModelFactory, IMapper mapper, ICsvService csvService, IHobbyManager hobbyManager)
    {
        _hobbyViewModelFactory = hobbyViewModelFactory;
        _mapper = mapper;
        _csvService = csvService;
        _hobbyManager = hobbyManager;        

        _hobbiesCollection = new ObservableCollection<IHobbyViewModel>();
        Hobbies = CollectionViewSource.GetDefaultView(_hobbiesCollection);
        Hobbies.Filter = FilterHobbies;
        ((ListCollectionView)Hobbies).CustomSort = Comparer<IHobbyViewModel>.Create(CompareHobbySortOrder);
        _hobbyManager.HobbiesChanged += HobbiesChangedEventHandler;

        AddHobbyCommand = new RelayCommand(AddEmptyHobby);
        CancelEditHobbyCommand = new GenericRelayCommand<HobbyViewModel>(CancelEditHobby, CanCancelEditHobby);
        DeleteHobbyCommand = new GenericRelayCommand<IHobbyViewModel>(DeleteHobby, CanDeleteHobby);
        ImportHobbiesCommand = new RelayCommand(ImportHobbies);
        ExportHobbiesCommand = new RelayCommand(ExportHobbies, CanExportHobbies);
        RemoveNotificationCommand = new GenericRelayCommand<NotificationMessage>(RemoveNotification);
        SaveHobbyCommand = new GenericRelayCommand<HobbyViewModel>(SaveHobby, CanSaveHobby);
        SortGridViewByColumnCommand = new GenericRelayCommand<string>(SortHobbyList);
        StartEditHobbyCommand = new GenericRelayCommand<HobbyViewModel>(StartEditHobby, CanStartEditHobby);

        SetDefaultHobbyListSorting();
        _ = LoadDataAsync();
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

    public bool IsGridViewSortOrderAscending
    {
        get
        {
            return _gridViewSortOrderIsAscending;
        }

        private set
        {
            _gridViewSortOrderIsAscending = value;
            RaisePropertyChanged(nameof(IsGridViewSortOrderAscending));
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
    public ICommand CancelEditHobbyCommand { get; }
    public ICommand DeleteHobbyCommand { get; }
    public ICommand ExportHobbiesCommand { get; }
    public ICommand ImportHobbiesCommand { get; }
    public ICommand RemoveNotificationCommand { get; }
    public ICommand SaveHobbyCommand { get; }
    public ICommand SortGridViewByColumnCommand { get; }
    public ICommand StartEditHobbyCommand { get; }

    #endregion

    #region CommandMethods 

    private static bool CanCancelEditHobby(HobbyViewModel hobby)
    {
        return true;
    }

    private static bool CanSaveHobby(HobbyViewModel hobby)
    {
        return hobby.CanSave();
    }

    private static bool CanStartEditHobby(HobbyViewModel hobby)
    {
        return !hobby.IsEditing;
    }

    private static void StartEditHobby(HobbyViewModel hobby)
    {
        if (!hobby.IsEditing)
        {
            hobby.StartEdit();
        }
    }

    private void AddEmptyHobby()
    {
        var newHobby = _hobbyViewModelFactory.CreateHobbyViewModel();
        newHobby.StartEdit();
        _hobbiesCollection.Add(newHobby);
    }

    private void CancelEditHobby(HobbyViewModel hobby)
    {
        if (hobby.IsEditing)
        {
            hobby.CancelEdit();
            _hobbiesCollection.Remove(hobby);
        }
    }

    private bool CanDeleteHobby(IHobbyViewModel hobby)
    {
        return hobby != null;
    }

    private bool CanExportHobbies()
    {
        return _hobbiesCollection.Count > 0;
    }

    private void DeleteHobby(IHobbyViewModel hobby)
    {
        if (CanDeleteHobby(hobby))
        {
            if (MessageBox.Show("Are you sure you want to delete this hobby?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _hobbyManager.DeleteHobby(hobby.Id);
                ShowNotification("Deleted hobby.");
            }
        }
    }

    private void ExportHobbies()
    {
        if (CanExportHobbies())
        {
            try
            {
                if (_csvService.TryWriteCsvFile(
                () =>
                {
                    List<string> rows = [_hobbiesCollection[0].HobbyHeaderNamesAsCsv()];

                    foreach (var hobby in _hobbiesCollection)
                    {
                        rows.Add(hobby.HobbyAsCSV());
                    }

                    return rows;
                }))
                {
                    ShowNotification("Exported hobbies successfully.");
                }
            }
            catch (Exception)
            {
                ShowNotification("An error occured during the export.");
            }
        }
    }

    private async Task ImportHobbies()
    {
        try
        {
            if (_csvService.TryReadCsvFile(out var csvFile))
            {
                int importedCount = 0;

                foreach (var row in csvFile.CsvRows)
                {
                    if (!row.TryGetCell(nameof(HobbyViewModel.Name), out var name) 
                        || !row.TryGetCell(nameof(HobbyViewModel.Description), out var description))
                    {
                        ShowNotification("Invalid column names in the CSV file.");
                        return;
                    }

                    if (!_hobbiesCollection.Any(x => x.Name == name.Value))
                    {
                        await _hobbyManager.CreateHobby(new Hobby(name: name!.Value, description: description!.Value));
                        importedCount++;
                    }
                }

                ShowNotification($"{importedCount} of {csvFile.CsvRows.Count} hobbies was imported.");
            }
        }
        catch (InvalidCsvFormatException)
        {
            ShowNotification("Invalid CSV file format.");
            return;
        }
    }

    private void RemoveNotification(NotificationMessage notification)
    {
        _notifications.Remove(notification);
    }

    private async Task SaveHobby(HobbyViewModel hobby)
    {
        if (CanSaveHobby(hobby))
        {
            if (_hobbyManager.HobbyExists(hobby.EditHobbyData!.EditName, excludeHobbyId: hobby.Id))
            {
                // TODO - Add an error notication type
                ShowNotification("A hobby with that name already exists");
                return;
            }

            var updatedHobby = hobby.SaveEdit();

            if (hobby.IsEmpty())
            {
                _hobbiesCollection.Remove(hobby);
                await _hobbyManager.CreateHobby(updatedHobby);
                ShowNotification("Hobby created.");
            }
            else
            {
                await _hobbyManager.UpdateHobby(updatedHobby);
                ShowNotification("Hobby updated.");
            }
        }
    }

    private void SortHobbyList(string columnName)
    {
        IsGridViewSortOrderAscending = !IsGridViewSortOrderAscending;
        GridViewSortedByColumn = columnName;
        Hobbies.Refresh();
    }

    #endregion

    #region Methods

    private void AddHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            var newHobby = _mapper.Map<IHobbyViewModel>(hobby);
            _hobbiesCollection.Add(newHobby);

            if (!IsLoadingData)
            {
                StartSafeUiActionAsync(newHobby.SetAsUpdated);             
            }
        }
    }

    private void ClearHobbies()
    {
        _hobbiesCollection.Clear();
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
            return IsGridViewSortOrderAscending ? result : -result;
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

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoadingData = true;
            await _hobbyManager.LoadDataAsync();
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
            var hobbyToRemove = _hobbiesCollection.FirstOrDefault(x => x.Id == hobby.Id);

            if (hobbyToRemove != null)
            {
                _hobbiesCollection.Remove(hobbyToRemove);
            }
        }
    }

    private void SetDefaultHobbyListSorting()
    {
        GridViewSortedByColumn = nameof(IHobbyViewModel.Name);
        IsGridViewSortOrderAscending = true;
        Hobbies.Refresh();
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

    private void StartSafeUiActionAsync(Action action)
    {
        // We must let the UI render the row completely first to avoid an error.
        Task.Run(async () =>
        {
            await Task.Delay(100);
            Application.Current.Dispatcher.Invoke(() =>
            {
                action();
            });
        });
    }

    #endregion
}
