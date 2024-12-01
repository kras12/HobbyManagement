using HobbyManagement.Viewmodels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace HobbyManagement.Controls.HobbyList.ViewModels;

public class DesignHobbyManagerViewModel
{
    private readonly ObservableCollection<DesignHobbyViewModel> _hobbiesCollection;

    private readonly ICollectionView _hobbies;

    public DesignHobbyManagerViewModel()
    {
        _hobbiesCollection = new()
        {
                new DesignHobbyViewModel("Hobby A", "A fine hobby."),
                new DesignHobbyViewModel("Hobby B", "It's better than nothing."),
                new DesignHobbyViewModel("Hobby C", "I have been doing it my whole life.")
        };
        _hobbies = CollectionViewSource.GetDefaultView(_hobbiesCollection);
    }

    public ICollectionView Hobbies => _hobbies;

    public string GridViewSortedByColumn => nameof(IHobbyViewModel.Name);

    public bool IsGridViewSortOrderAscending => true;
}
