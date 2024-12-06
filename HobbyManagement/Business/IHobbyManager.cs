using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagement.Business;

/// <summary>
/// Interface for a hobby manager handling business logic. 
/// </summary>
public interface IHobbyManager
{
    /// <summary>
    /// Collection changed event for hobbies.
    /// </summary>
    event NotifyCollectionChangedEventHandler HobbiesChanged;

    /// <summary>
    /// A collection of hobbies being managed.
    /// </summary>
    ReadOnlyObservableCollection<IHobby> Hobbies { get; }

    /// <summary>
    /// Creates a new hobby in the database and adds it to the collection.
    /// </summary>
    /// <param name="hobby">The hobby to add.</param>
    /// <returns><see cref="Task"/></returns>
    Task CreateHobby(IHobby hobby);

    /// <summary>
    /// Deletes a hobby from the database and removes it from the collection. 
    /// </summary>
    /// <param name="Id">The ID of the hobby.</param>
    /// <returns><see cref="Task"/></returns>
    Task DeleteHobby(int Id);

    /// <summary>
    /// Checks whether a hobby exists.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="excludeHobbyId">An optional ID of a hobby to ignore.</param>
    /// <returns>True if the hobby exists.</returns>
    bool HobbyExists(string name, int? excludeHobbyId = null);

    /// <summary>
    /// A task that loads the data of the manager. 
    /// This includes fetching hobbies from the database.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    Task LoadDataAsync();

    /// <summary>
    /// Updates a hobby in the database and in the collection. 
    /// </summary>
    /// <param name="hobby">The hobby to update.</param>
    /// <returns><see cref="Task"/></returns>
    Task UpdateHobby(IHobby hobby);
}