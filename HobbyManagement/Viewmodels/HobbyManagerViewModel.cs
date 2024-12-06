using AutoMapper;
using HobbyManagement.Business;
using HobbyManagement.Commands;
using HobbyManagement.Services;
using HobbyManagement.Services.Csv;
using HobbyManagement.Services.Csv.Error;
using HobbyManagment.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace HobbyManagement.Viewmodels;

/// <summary>
/// View model for a hobby manager.
/// </summary>
public class HobbyManagerViewModel : ObservableObjectBase, IHobbyManagerViewModel
{
    #region Constants

    /// <summary>
    /// The number of seconds that the save hobby animation plays for an edited row. 
    /// </summary>
    private const int SaveHobbyRowAnimationDurationInSeconds = 2;

    #endregion

    #region Fields

    /// <summary>
    /// The injected csv service.
    /// </summary>
    private readonly ICsvService _csvService;

    /// <summary>
    /// An observable collection that is serving property <see cref="Hobbies"/> with data.
    /// </summary>
    private readonly ObservableCollection<IHobbyViewModel> _hobbiesCollection;

    /// <summary>
    /// The injected hobby manager.
    /// </summary>
    private readonly IHobbyManager _hobbyManager;

    /// <summary>
    /// The injected factory to create <see cref="IHobbyViewModel"/> objects. 
    /// </summary>
    private readonly IHobbyViewModelFactory _hobbyViewModelFactory;

    /// <summary>
    /// The injected data mapper.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Backing field for property <see cref="GridViewSortedByColumn"/>.
    /// </summary>
    private string _gridViewSortedByColumn = "";

    /// <summary>
    /// Backing field for property <see cref="IsGridViewSortOrderAscending"/>.
    /// </summary>
    private bool _isGridViewSortOrderAscending = true;

    /// <summary>
    /// Backing field for property <see cref="Hobbies"/>.
    /// </summary>
    private ICollectionView _hobbies = default!;

    /// <summary>
    /// Backing field for property <see cref="IsLoadingData"/>.
    /// </summary>
    private bool _isLoadingData;

    /// <summary>
    /// Backing field for property <see cref="Notifications"/>.
    /// </summary>
    private ObservableCollection<NotificationMessage> _notifications = new();

    /// <summary>
    /// Backing field for property <see cref="SearchText"/>.
    /// </summary>
    private string _searchText = "";

    /// <summary>
    /// The injected service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// The number of milliseconds left of the save hobby row animation. 
    /// This value is used to delay operations like grid view row sorting that can't be done while an animation is running.
    /// </summary>
    private int _milliSecondsLeftOfSaveHobbyRowAnimation;

    /// <summary>
    /// The timer used when delaying operations like grid view row sorting that can't be done while an animation is running.
    /// </summary>
    private DispatcherTimer? _hobbyRowAnimationTimer = null;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hobbyViewModelFactory">The injected factory to create <see cref="IHobbyViewModel"/> objects. </param>
    /// <param name="mapper">The injected data mapper.</param>
    /// <param name="csvService">The injected csv service.</param>
    /// <param name="hobbyManager">The injected hobby manager.</param>
    /// <param name="serviceProvider">The injected service provider.</param>
    public HobbyManagerViewModel(IHobbyViewModelFactory hobbyViewModelFactory, IMapper mapper, ICsvService csvService, 
        IHobbyManager hobbyManager, IServiceProvider serviceProvider)
    {
        _hobbyViewModelFactory = hobbyViewModelFactory;
        _mapper = mapper;
        _csvService = csvService;
        _hobbyManager = hobbyManager;
        _serviceProvider = serviceProvider;

        _hobbiesCollection = new ObservableCollection<IHobbyViewModel>();
        Hobbies = CollectionViewSource.GetDefaultView(_hobbiesCollection);
        Hobbies.Filter = FilterHobbies;
        ((ListCollectionView)Hobbies).CustomSort = Comparer<IHobbyViewModel>.Create(CompareHobbySortOrder);
        _hobbyManager.HobbiesChanged += HobbiesChangedEventHandler;

        CreateHobbyCommand = new RelayCommand(AddEmptyHobby);
        CancelEditHobbyCommand = new GenericRelayCommand<HobbyViewModel>(CancelEditHobby, CanCancelEditHobby);
        DeleteHobbyCommand = new GenericRelayCommand<IHobbyViewModel>(DeleteHobby, CanDeleteHobby);
        ImportHobbiesCommand = new RelayCommand(ImportHobbies);
        ExportHobbiesCommand = new RelayCommand(ExportHobbies, CanExportHobbies);
        RemoveNotificationCommand = new GenericRelayCommand<NotificationMessage>(RemoveNotification);
        SaveHobbyCommand = new GenericRelayCommand<HobbyViewModel>(SaveHobby, CanSaveHobby);
        SortHobbiesListByColumnCommand = new GenericRelayCommand<string>(SortHobbyList, CanSortHobbyList);
        StartEditHobbyCommand = new GenericRelayCommand<HobbyViewModel>(StartEditHobby, CanStartEditHobby);

        SetDefaultHobbyListSorting();
        _ = LoadDataAsync();
    }

    #endregion

    #region Properties

    /// <summary>
    /// The column that the hobbies list is sorted by. 
    /// </summary>
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

    /// <summary>
    /// The view that handles the listing, sorting and filtering of hobbies. 
    /// </summary>
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

    /// <summary>
    /// Returns true if the sort order of the hobbies list is ascending. 
    /// </summary>
    public bool IsGridViewSortOrderAscending
    {
        get
        {
            return _isGridViewSortOrderAscending;
        }

        private set
        {
            _isGridViewSortOrderAscending = value;
            RaisePropertyChanged(nameof(IsGridViewSortOrderAscending));
        }
    }

    /// <summary>
    /// Returns true if new data is being loaded. 
    /// </summary>
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

    /// <summary>
    /// A collection of notifications that are being shown to the user. 
    /// </summary>
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

    /// <summary>
    /// The search text used for filtering hobbies in the hobbies list. 
    /// </summary>
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

    /// <summary>
    /// The command to create a new empty hobby in edit mode. 
    /// Saving the hobby will create a new hobby. 
    /// </summary>
    public ICommand CreateHobbyCommand { get; } 

    /// <summary>
    /// The command to cancel the process of editing a hobby. 
    /// </summary>
    public ICommand CancelEditHobbyCommand { get; }

    /// <summary>
    /// The command to delete a hobby. 
    /// </summary>
    public ICommand DeleteHobbyCommand { get; }

    /// <summary>
    /// The command to export all hobbies to a csv file. 
    /// </summary>
    public ICommand ExportHobbiesCommand { get; }

    /// <summary>
    /// The command to import hobbies from a csv file. 
    /// </summary>
    public ICommand ImportHobbiesCommand { get; }

    /// <summary>
    /// The command to remove a user notification message from the collection. 
    /// A user manually closing a notification will trigger this command. 
    /// </summary>
    public ICommand RemoveNotificationCommand { get; }

    /// <summary>
    /// The command to save a hobby that is being edited.
    /// </summary>
    public ICommand SaveHobbyCommand { get; }

    /// <summary>
    /// The command to sort the hobbies list by a column.
    /// </summary>
    public ICommand SortHobbiesListByColumnCommand { get; }

    /// <summary>
    /// The command to start editing a hobby. 
    /// </summary>
    public ICommand StartEditHobbyCommand { get; }

    #endregion

    #region CommandMethods 

    /// <summary>
    /// Checks whether the <see cref="SortHobbiesListByColumnCommand"/> command can be executed.
    /// </summary>
    /// <param name="column">The name of the column to sort by.</param>
    /// <returns>True if the command can be executed.</returns>
    private bool CanSortHobbyList(string column)
    {
        return _milliSecondsLeftOfSaveHobbyRowAnimation <= 0;
    }

    /// <summary>
    /// Checks whether the <see cref="CancelEditHobbyCommand"/> command can be executed.
    /// </summary>
    /// <param name="hobby">The hobby that is being edited.</param>
    /// <returns>True if the command can be executed.</returns>
    private static bool CanCancelEditHobby(HobbyViewModel hobby)
    {
        return true;
    }

    /// <summary>
    /// Checks whether the <see cref="SaveHobbyCommand"/> command can be executed.
    /// </summary>
    /// <param name="hobby">The hobby that is being edited.</param>
    /// <returns>True if the command can be executed.</returns>
    private static bool CanSaveHobby(HobbyViewModel hobby)
    {
        return hobby.CanSave();
    }

    /// <summary>
    /// Checks whether the <see cref="StartEditHobbyCommand"/> command can be executed.
    /// </summary>
    /// <param name="hobby">The hobby to edit.</param>
    /// <returns>True if the command can be executed.</returns>
    private static bool CanStartEditHobby(HobbyViewModel hobby)
    {
        return !hobby.IsEditing;
    }

    /// <summary>
    /// Starts the editing process of a hobby. 
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="StartEditHobbyCommand"/> command.</remarks>
    /// <param name="hobby">The hobby to edit.</param>
    private static void StartEditHobby(HobbyViewModel hobby)
    {
        if (!hobby.IsEditing)
        {
            hobby.StartEdit();
        }
    }

    /// <summary>
    /// Creates an empty placeholder hobby and starts the edit mode. Saving the changes creates a new hobby. 
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="StartEditHobbyCommand"/> command.</remarks>
    private void AddEmptyHobby()
    {
        var newHobby = _hobbyViewModelFactory.CreateHobbyViewModel();
        newHobby.StartEdit();
        _hobbiesCollection.Add(newHobby);
    }

    /// <summary>
    /// Cancels the editing process for a hobby.
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="CancelEditHobbyCommand"/> command.</remarks>
    /// <param name="hobby">The hobby being edited.</param>
    private void CancelEditHobby(HobbyViewModel hobby)
    {
        if (hobby.IsEditing)
        {
            hobby.CancelEdit();

            if (hobby.IsEmpty())
            {
                _hobbiesCollection.Remove(hobby);
            }
        }
    }

    /// <summary>
    /// Checks whether the <see cref="DeleteHobbyCommand"/> command can be executed.
    /// </summary>
    /// <param name="hobby">The hobby to delete.</param>
    /// <returns>True if the command can be executed.</returns>
    private bool CanDeleteHobby(IHobbyViewModel hobby)
    {
        return hobby != null;
    }

    /// <summary>
    /// Checks whether the <see cref="ExportHobbiesCommand"/> command can be executed.
    /// </summary>
    /// <returns>True if the command can be executed.</returns>
    private bool CanExportHobbies()
    {
        return _hobbiesCollection.Count > 0;
    }

    /// <summary>
    /// Deletes a hobby.
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="DeleteHobbyCommand"/> command.</remarks>
    /// <param name="hobby">The hobby to delete.</param>
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

    /// <summary>
    /// Exports all hobbies to a csv file on disk. 
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="ExportHobbiesCommand"/> command.</remarks>
    private void ExportHobbies()
    {
        if (CanExportHobbies())
        {
            try
            {
                if (_csvService.TryWriteCsvFile(
                () =>
                {
                    List<string> rows = [_hobbiesCollection[0].HobbyAttributesAsCsvHeader()];

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

    /// <summary>
    /// Imports hobbies from a csv file on disk. Existing hobbies will be ignored. 
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="ImportHobbiesCommand"/> command.</remarks>
    /// <returns><see cref="Task"/></returns>
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
                        var newHobby = _serviceProvider.GetRequiredService<IHobby>();
                        newHobby.Name = name!.Value;
                        newHobby.Description = description!.Value;

                        await _hobbyManager.CreateHobby(newHobby);
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

    /// <summary>
    /// Removes a user notification message from the collection. 
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="RemoveNotificationCommand"/> command.</remarks>
    /// <param name="notification">The notification to remove.</param>
    private void RemoveNotification(NotificationMessage notification)
    {
        _notifications.Remove(notification);
    }


    /// <summary>
    /// Saves a hobby that is being edited.
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="SaveHobbyCommand"/> command.</remarks>
    /// <param name="hobby">The hobby to save.</param>
    private async Task SaveHobby(HobbyViewModel hobby)
    {
        if (CanSaveHobby(hobby))
        {
            if (_hobbyManager.HobbyExists(hobby.EditHobbyData!.EditName, excludeHobbyId: hobby.Id))
            {
                // TODO - Add an error notification type
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

            _milliSecondsLeftOfSaveHobbyRowAnimation = SaveHobbyRowAnimationDurationInSeconds * 1000 + 100;

            if (_hobbyRowAnimationTimer == null)
            {
                _hobbyRowAnimationTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
                _hobbyRowAnimationTimer.Tick += (sender, e) =>
                {
                    _milliSecondsLeftOfSaveHobbyRowAnimation -= 100;

                    if (_milliSecondsLeftOfSaveHobbyRowAnimation <= 0)
                    {
                        _hobbyRowAnimationTimer.Stop();
                        _hobbyRowAnimationTimer = null;
                        CommandManager.InvalidateRequerySuggested();
                    }                    
                };
                _hobbyRowAnimationTimer.Start();
            }           
        }
    }

    /// <summary>
    /// Sorts the hobby list by a column. 
    /// </summary>
    /// <remarks>Performs the operations of the <see cref="SortHobbiesListByColumnCommand"/> command.</remarks>
    /// <param name="columnName">The name of the column to sort by.</param>
    private void SortHobbyList(string columnName)
    {
        IsGridViewSortOrderAscending = !IsGridViewSortOrderAscending;
        GridViewSortedByColumn = columnName;
        Hobbies.Refresh();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Converts and adds a collection of <see cref="IHobby>"/> to the collection of hobbies.
    /// </summary>
    /// <remarks>This method is used for importing hobbies provided by the hobby manager.</remarks>
    /// <param name="hobbies">The hobbies to add.</param>
    private void AddHobbies(List<IHobby> hobbies)
    {
        foreach (IHobby hobby in hobbies)
        {
            var newHobby = _mapper.Map<IHobbyViewModel>(hobby);
            _hobbiesCollection.Add(newHobby);

            if (!IsLoadingData)
            {
                // We must let the UI render the row completely first to avoid an error.
                StartSafeUiActionAsync(newHobby.SetAsUpdated);             
            }
        }
    }

    /// <summary>
    /// Clears the hobbies collection.
    /// </summary>
    private void ClearHobbies()
    {
        _hobbiesCollection.Clear();
    }

    /// <summary>
    /// Callback method used by a <see cref="Comparer{T}"/> to perform a sort order comparison for hobbies. 
    /// Takes into account both the column and the sort order.
    /// </summary>
    /// <param name="hobbyA">The first hobby to compare.</param>
    /// <param name="hobbyB">The second hobby to compare.</param>
    /// <returns></returns>
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

    /// <summary>
    /// A filter method used by an <see cref="ICollectionView"/> to filter hobbies according to a search text. 
    /// It filters by both the name and the description attributes of a hobby.
    /// </summary>
    /// <param name="item">The hobby being evaluated.</param>
    /// <returns>True if the evaluted hobby matched the criteria.</returns>
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

    /// <summary>
    /// Event handler for changes in the hobbies collection in the hobby manager. 
    /// Changes are synchronized with the class collection <see cref="_hobbiesCollection"/>.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">Event parameter that contains the changes.</param>
    private void HobbiesChangedEventHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddHobbies(e.NewItems!.Cast<IHobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Remove:
                RemoveHobbies(e.OldItems!.Cast<IHobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Reset:
                ClearHobbies();
                AddHobbies(_hobbyManager.Hobbies.ToList());
                break;

            case NotifyCollectionChangedAction.Replace:
                RemoveHobbies(e.OldItems!.Cast<IHobby>().ToList());
                AddHobbies(e.NewItems!.Cast<IHobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Move:
                MoveHobby(e.OldStartingIndex, e.NewStartingIndex);
                break;
        }
    }

    /// <summary>
    /// Instructs the hobby manager to load its data.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
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

    /// <summary>
    /// Moves a hobby in <see cref="_hobbiesCollection"/> to a new position.
    /// </summary>
    /// <param name="oldStartingIndex">The index of the old position.</param>
    /// <param name="newStartingIndex">The index of the position to move the hobby to.</param>
    private void MoveHobby(int oldStartingIndex, int newStartingIndex)
    {
        _hobbiesCollection.Move(oldStartingIndex, newStartingIndex);
    }

    /// <summary>
    /// Removes hobbies from the <see cref="_hobbies"/> collection.
    /// </summary>
    /// <param name="hobbies">The hobbies to remove.</param>
    private void RemoveHobbies(List<IHobby> hobbies)
    {
        foreach (IHobby hobby in hobbies)
        {
            var hobbyToRemove = _hobbiesCollection.FirstOrDefault(x => x.Id == hobby.Id);

            if (hobbyToRemove != null)
            {
                _hobbiesCollection.Remove(hobbyToRemove);
            }
        }
    }

    /// <summary>
    /// Sets the default sort settings for the hobby list. 
    /// </summary>
    private void SetDefaultHobbyListSorting()
    {
        GridViewSortedByColumn = nameof(IHobbyViewModel.Name);
        IsGridViewSortOrderAscending = true;
        Hobbies.Refresh();
    }

    /// <summary>
    /// Shows a notification to the user by adding a notification to the <see cref="Notifications"/> collection.
    /// </summary>
    /// <param name="message">The message of the notification.</param>
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

    /// <summary>
    /// Performs an action after a small delay. This to give the UI thread time to process 
    /// important UI related operations that must be performed before the action can be safely executed.
    /// </summary>
    /// <param name="action"></param>
    private void StartSafeUiActionAsync(Action action)
    {        
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
