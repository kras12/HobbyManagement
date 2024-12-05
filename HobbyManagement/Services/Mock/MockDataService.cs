using HobbyManagement.Business;
using HobbyManagment.Data.Repositories;

namespace HobbyManagement.Services.Mock;

/// <summary>
/// A service used to seed mock data to the database.
/// </summary>
public class MockDataService : IMockDataService
{
    #region Fields

    /// <summary>
    /// Injected repository for hobbies.
    /// </summary>
    private readonly IHobbiesRepository _hobbiesRepository;

    /// <summary>
    /// Injected hobby manager.
    /// </summary>
    private readonly IHobbyManager _hobbyManager;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hobbiesRepository">Injected repository for hobbies.</param>
    /// <param name="hobbyManager"> Injected hobby manager.</param>
    public MockDataService(IHobbiesRepository hobbiesRepository, IHobbyManager hobbyManager)
    {
        _hobbiesRepository = hobbiesRepository;
        _hobbyManager = hobbyManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Seeds mock hobbies to the database if it doesn't contain any hobbies.
    /// </summary>
    /// <returns></returns>
    public async Task TrySeedHobbies()
    {
        try
        {
            if (await _hobbiesRepository.HobbiesCount() == 0)
            {
                List<Hobby> seedHobbies = new()
            {
                new Hobby(name: "Weight Training", description: "Weight training at the gym."),
                new Hobby(name: "Movies and TV-series", description: "Occasionally watching movies and TV-series."),
                new Hobby(name: "Programming", description: "Programming with C# .Net.")
            };

                foreach (var seedHobby in seedHobbies)
                {
                    await _hobbyManager.CreateHobby(seedHobby);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while seeding hobbies: {ex.Message}");
            throw;
        }
    }

    #endregion
}
