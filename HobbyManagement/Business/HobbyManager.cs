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

    public async Task AddHobby(Hobby hobby)
    {
        if (HobbyExists(hobby.Name))
        {
            throw new InvalidOperationException("A hobby with that name already exists");
        }

        if (!await _hobbiesRepository.HobbyExists(hobby.Name))
        {
            var newHobby = await _hobbiesRepository.CreateHobbyAsync(_mapper.Map<HobbyEntity>(hobby));
            _hobbies.Add(_mapper.Map<Hobby>(newHobby));
        }
    }

    public async Task DeleteHobby(int Id)
    {
        var targetHobby = _hobbies.FirstOrDefault(x => x.Id == Id);

        if (targetHobby == null)
        {
            throw new InvalidOperationException("The hobby was not found.");
        }

        if (await _hobbiesRepository.HobbyExists(targetHobby.Name))
        {
            await _hobbiesRepository.DeleteHobbyAsync(_mapper.Map<HobbyEntity>(targetHobby));
            _hobbies.Remove(targetHobby);
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

    public async Task UpdateHobby(Hobby hobby)
    {
        var targetHobby = _hobbies.FirstOrDefault(x => x.Id == hobby.Id);

        if (targetHobby == null)
        {
            throw new InvalidOperationException("The hobby was not found.");
        }

        var updatedHobby = await _hobbiesRepository.Update(_mapper.Map<HobbyEntity>(targetHobby));
        _mapper.Map(source: _mapper.Map<Hobby>(updatedHobby), destination: targetHobby);
    }

    private async Task SeedDataAsync()
    {
        // Todo - Move to seeding class
        List<Hobby> seedHobbies = new()
            {
                new Hobby(id: 1, name: "Weight Training", description: "Weight training at the gym."),
                new Hobby(id: 2, name: "Movies and TV-series", description: "Occasionally watching movies and TV-series."),
                new Hobby(id: 3, name: "Programming", description: "Programming with C# .Net.")
            };

        foreach (var seedHobby in seedHobbies)
        {
            await AddHobby(_mapper.Map<Hobby>(seedHobby));
        }
    }
}
