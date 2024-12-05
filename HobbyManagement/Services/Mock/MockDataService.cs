using HobbyManagement.Business;
using HobbyManagment.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

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

    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hobbiesRepository">Injected repository for hobbies.</param>
    /// <param name="hobbyManager"> Injected hobby manager.</param>
    public MockDataService(IHobbiesRepository hobbiesRepository, IHobbyManager hobbyManager, IServiceProvider serviceProvider)
    {
        _hobbiesRepository = hobbiesRepository;
        _hobbyManager = hobbyManager;
        _serviceProvider = serviceProvider;
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
                List<(string hobbyName, string hobbyDescription)> seedHobbies = new()
                {
                    ("Weight Training", "Weight training at the gym."),
                    ("Movies and TV-series",
                    "Occasionally watching movies and TV-series."),
                    ("Programming", "Programming with C# .Net.")
                };

                foreach (var seedHobby in seedHobbies)
                {
                    await _hobbyManager.CreateHobby(CreateIHobby(seedHobby.hobbyName, seedHobby.hobbyDescription));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while seeding hobbies: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Creates an <see cref="IHobby"/> by using the service provider.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="description">The description of the hobby.</param>
    /// <returns></returns>
    private IHobby CreateIHobby(string name, string description)
    {
        var hobby = _serviceProvider.GetRequiredService<IHobby>();
        hobby.Name = name;
        hobby.Description = description;
        return hobby;
    }

    #endregion
}
