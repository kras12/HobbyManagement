using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HobbyManagement.Viewmodels;

public interface IHobbyManagerViewModel
{
    /// <summary>
    /// The command to create a new empty hobby in edit mode. 
    /// Saving the hobby will create a new hobby. 
    /// </summary>
    ICommand AddHobbyCommand { get; }

    /// <summary>
    /// The command to cancel the process of editing a hobby. 
    /// </summary>
    ICommand CancelEditHobbyCommand { get; }

    /// <summary>
    /// The command to delete a hobby. 
    /// </summary>
    ICommand DeleteHobbyCommand { get; }

    /// <summary>
    /// The column that the hobbies list is sorted by. 
    /// </summary>
    string GridViewSortedByColumn { get; }

    /// <summary>
    /// The view that handles the listing, sorting and filtering of hobbies. 
    /// </summary>
    ICollectionView Hobbies { get; }

    /// <summary>
    /// Returns true if the sort order of the hobbies list is ascending. 
    /// </summary>
    bool IsGridViewSortOrderAscending { get; }

    /// <summary>
    /// Returns true if new data is being loaded. 
    /// </summary>
    bool IsLoadingData { get; }

    /// <summary>
    /// A collection of notifications that are being shown to the user. 
    /// </summary>
    ObservableCollection<NotificationMessage> Notifications { get; set; }

    /// <summary>
    /// The command to save a hobby that is being edited.
    /// </summary>
    ICommand SaveHobbyCommand { get; }

    /// <summary>
    /// The search text used for filtering hobbies in the hobbies list. 
    /// </summary>
    string SearchText { get; set; }

    /// <summary>
    /// The command to sort the hobbies list by a column.
    /// </summary>
    ICommand SortGridViewByColumnCommand { get; }

    /// <summary>
    /// The command to start editing a hobby. 
    /// </summary>
    ICommand StartEditHobbyCommand { get; }
}