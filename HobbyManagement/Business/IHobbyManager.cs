using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagement.Business;

/// <summary>
/// Interface for a hobby manager handling business logic. 
/// </summary>
public interface IHobbyManager
{
    event NotifyCollectionChangedEventHandler HobbiesChanged;
    ReadOnlyObservableCollection<IHobby> Hobbies { get; }
    Task CreateHobby(IHobby hobby);
    Task DeleteHobby(int Id);
    bool HobbyExists(string name, int? excludeHobbyId = null);
    Task LoadDataAsync();
    Task UpdateHobby(IHobby hobby);
}