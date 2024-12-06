
namespace HobbyManagement.Services.Mock;

/// <summary>
/// Interface for a service used to seed mock data to the database.
/// </summary>
public interface IMockDataService
{
    /// <summary>
    /// Seeds mock hobbies to the database if it doesn't contain any hobbies.
    /// </summary>
    /// <returns><see cref="Task"/><returns>
    Task TrySeedHobbies();
}