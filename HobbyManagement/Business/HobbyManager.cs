using AutoMapper;
using HobbyManagment.Data;
using HobbyManagment.Data.Database.Models;
using HobbyManagment.Data.Repositories;
using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagement.Business;

public class HobbyManager : ObservableObjectBase, IHobbyManager
{
    private readonly ObservableCollection<Hobby> _hobbies = [];
    private readonly IHobbiesRepository _hobbiesRepository;
    private readonly IMapper _mapper;
    public HobbyManager(IHobbiesRepository hobbiesRepository, IMapper mapper)
    {
        Hobbies = new ReadOnlyObservableCollection<Hobby>(_hobbies);
        _hobbiesRepository = hobbiesRepository;
        _mapper = mapper;
    }

    public event NotifyCollectionChangedEventHandler HobbiesChanged
    {
        add { _hobbies.CollectionChanged += value; }
        remove { _hobbies.CollectionChanged -= value; }
    }

    public ReadOnlyObservableCollection<Hobby> Hobbies { get; }

    public async Task CreateHobby(Hobby hobby)
    {
        if (HobbyExists(hobby.Name))
        {
            throw new InvalidOperationException("A hobby with that name already exists");
        }

        try
        {

            if (!await _hobbiesRepository.HobbyExists(hobby.Name))
            {
                var newHobby = await _hobbiesRepository.CreateHobbyAsync(_mapper.Map<HobbyEntity>(hobby));
                _hobbies.Add(_mapper.Map<Hobby>(newHobby));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while creating a hobby: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteHobby(int Id)
    {
        if (Id <= 0)
        {
            throw new ArgumentOutOfRangeException($"The hobby ID must be larger than zero.");
        }

        var targetHobby = _hobbies.FirstOrDefault(x => x.Id == Id);

        if (targetHobby == null)
        {
            throw new InvalidOperationException("The hobby was not found.");
        }

        try
        {
            if (await _hobbiesRepository.HobbyExists(targetHobby.Name))
            {
                await _hobbiesRepository.DeleteHobbyAsync(_mapper.Map<HobbyEntity>(targetHobby));
                _hobbies.Remove(targetHobby);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while deleting a hobby: {ex.Message}");
            throw;
        }
    }

    public async Task<int> HobbiesCount()
    {
        try
        {
            return await _hobbiesRepository.HobbiesCount();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while getting hobbies count: {ex.Message}");
            throw;
        }
    }

    public bool HobbyExists(string name, int? excludeHobbyId = null)
    {
        var query = _hobbies
            .Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .AsQueryable();

        if (excludeHobbyId != null)
        {
            query = query
                .Where(x => x.Id != excludeHobbyId.Value);
        }

        return query.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task LoadDataAsync()
    {
        try
        {
            var hobbies = await _hobbiesRepository.GetAllAsync();

            if (hobbies.Count > 0)
            {
                foreach (var hobby in hobbies)
                {
                    _hobbies.Add(_mapper.Map<Hobby>(hobby));
                }
            }
            else
            {
                await SeedDataAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while loading hobbies: {ex.Message}");
            throw;
        }
        
    }

    public async Task UpdateHobby(Hobby hobby)
    {
        try
        {
            var targetHobby = _hobbies.FirstOrDefault(x => x.Id == hobby.Id);

            if (targetHobby == null)
            {
                throw new InvalidOperationException("The hobby was not found.");
            }

            var updatedHobby = await _hobbiesRepository.Update(_mapper.Map<HobbyEntity>(hobby));
            _mapper.Map(source: updatedHobby, destination: targetHobby);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while updating a hobby: {ex.Message}");
            throw;
        }
    }

    private async Task SeedDataAsync()
    {
        try
        {


            // Todo - Move to seeding class
            List<Hobby> seedHobbies = new()
            {
                new Hobby(name: "Weight Training", description: "Weight training at the gym."),
                new Hobby(name: "Movies and TV-series", description: "Occasionally watching movies and TV-series."),
                new Hobby(name: "Programming", description: "Programming with C# .Net.")
            };

            foreach (var seedHobby in seedHobbies)
            {
                await CreateHobby(_mapper.Map<Hobby>(seedHobby));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while seeding hobbies: {ex.Message}");
            throw;
        }
    }
}
