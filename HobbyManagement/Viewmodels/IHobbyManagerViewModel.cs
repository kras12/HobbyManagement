using System.ComponentModel;
using System.Windows.Input;

namespace HobbyManagement.Viewmodels
{
    public interface IHobbyManagerViewModel
    {
        ICommand AddHobbyCommand { get; }
        ICommand DeleteHobbyCommand { get; }
        string GridViewSortedByColumn { get; }
        bool GridViewSortOrderIsAscending { get; }
        ICollectionView HobbiesView { get; }
        bool IsLoadingData { get; }
        string SearchText { get; set; }
        ICommand SortGridViewByColumnCommand { get; }
    }
}