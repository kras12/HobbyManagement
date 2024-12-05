using HobbyManagment.Data;
using HobbyManagment.Data.Repositories;

namespace HobbyManagement.Services.Mock;

public class MockDataService : IMockDataService
{
    private readonly IHobbiesRepository _hobbiesRepository;

    public MockDataService(IHobbiesRepository hobbiesRepository)
    {
        _hobbiesRepository = hobbiesRepository;
    }

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
}
