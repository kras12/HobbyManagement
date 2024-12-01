using System.ComponentModel;
using System.Windows.Input;

namespace HobbyManagement.Viewmodels
{
    public interface IHobbyManagerViewModel
    {
        ICommand AddHobbyCommand { get; }
        ICommand CancelEditHobbyCommand { get; }
        ICommand DeleteHobbyCommand { get; }
        string GridViewSortedByColumn { get; }
        ICollectionView Hobbies { get; }
        bool IsGridViewSortOrderAscending { get; }
        bool IsLoadingData { get; }
        ICommand SaveHobbyCommand { get; }
        string SearchText { get; set; }
        ICommand SortGridViewByColumnCommand { get; }
        ICommand StartEditHobbyCommand { get; }
    }
}