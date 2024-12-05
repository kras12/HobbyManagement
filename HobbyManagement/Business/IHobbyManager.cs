using HobbyManagment.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagement.Business;

public interface IHobbyManager
{
    event NotifyCollectionChangedEventHandler HobbiesChanged;
    ReadOnlyObservableCollection<Hobby> Hobbies { get; }
    Task CreateHobby(Hobby hobby);
    Task DeleteHobby(int Id);
    bool HobbyExists(string name, int? excludeHobbyId = null);
    Task LoadDataAsync();
    Task UpdateHobby(Hobby hobby);
}