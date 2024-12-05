
namespace HobbyManagement.Services.Mock;

/// <summary>
/// Interface for a service used to seed mock data to the database.
/// </summary>
public interface IMockDataService
{
    Task TrySeedHobbies();
}