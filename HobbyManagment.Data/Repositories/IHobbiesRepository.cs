using HobbyManagment.Data.Database.Models;

namespace HobbyManagment.Data.Repositories;

public interface IHobbiesRepository
{
    /// <summary>
    /// Adds a hobby to the database.
    /// </summary>
    /// <param name="hobby">The hobby to add.</param>
    /// <returns>The created <see cref="HobbyEntity"/>.</returns>
    public Task<HobbyEntity> CreateHobbyAsync(HobbyEntity hobby);

    /// <summary>
    /// Deletes a hobby from the database.
    /// </summary>
    /// <param name="hobby">The hobby to delete.</param>
    /// <returns><see cref="Task"/></returns>
    public Task DeleteHobbyAsync(HobbyEntity hobby);

    /// <summary>
    /// Fetches all hobbies from the database.
    /// </summary>
    /// <returns>A collection of <see cref="HobbyEntity"/>.</returns>
    public Task<List<HobbyEntity>> GetAllAsync();

    /// <summary>
    /// Attempts to fetch a hobby by ID.
    /// </summary>
    /// <param name="id">The ID of the hobby.</param>
    /// <returns>The found <see cref="HobbyEntity"/> if the operation was successful.</returns>
    public Task<HobbyEntity?> GetAsync(int id);

    /// <summary>
    /// Returns the number of hobbies in the database.
    /// </summary>
    /// <returns>The number of hobbies in the form of an <see cref="int"/>.</returns>
    public Task<int> HobbiesCount();

    /// <summary>
    /// Checks whether a hobby exists in the database.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <returns>True if the hobby exists.</returns>
    public Task<bool> HobbyExists(string name);

    /// <summary>
    /// Updates a hobby in the database.
    /// </summary>
    /// <param name="hobby">The hobby to update.</param>
    /// <returns>The updated <see cref="HobbyEntity"/>.</returns>
    public Task<HobbyEntity> Update(HobbyEntity hobby);
}