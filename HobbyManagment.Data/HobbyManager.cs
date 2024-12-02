using AutoMapper;
using HobbyManagement.Mapping;
using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagment.Data;

public class HobbyManager : ObservableObjectBase
{
    private readonly ObservableCollection<Hobby> _hobbies = [];
    private readonly IMapper _mapper;
    public HobbyManager()
    {
        Hobbies = new ReadOnlyObservableCollection<Hobby>(_hobbies);
        var config = new MapperConfiguration(x => x.AddProfile<AutoMapperProfile>());
        _mapper = config.CreateMapper();
    }

    public event NotifyCollectionChangedEventHandler HobbiesChanged
    {
        add { _hobbies.CollectionChanged += value; }
        remove {  _hobbies.CollectionChanged -= value; }
    }

    public ReadOnlyObservableCollection<Hobby> Hobbies { get; }

    public void AddHobby(Hobby hobby)
    {
        if (HobbyExists(hobby.Name))
        {
            throw new InvalidOperationException("A hobby with that name already exists");
        }

        hobby.Id = GetNextHobbyId();
        _hobbies.Add(hobby);
    }

    public void ClearHobbies()
    {
        _hobbies.Clear();
    }

    public void DeleteHobby(int Id)
    {
        var targetHobby = _hobbies.FirstOrDefault(x => x.Id == Id);

        if (targetHobby == null)
        {
            throw new InvalidOperationException("The hobby was not found.");
        }

        _hobbies.Remove(targetHobby);
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

    public Task LoadData()
    {
        // TODO - Fetch data from a database
        _hobbies.Add(new Hobby(id: 1, name: "Weight Training", description: "Weight training at the gym."));
        _hobbies.Add(new Hobby(id: 2, name: "Movies and TV-series", description: "Occasionally watching movies and TV-series."));
        _hobbies.Add(new Hobby(id: 3, name: "Programming", description: "Programming with C# .Net."));

        return Task.CompletedTask;
    }

    public void UpdateHobby(Hobby hobby)
    {
        var targetHobby = _hobbies.FirstOrDefault(x => x.Id == hobby.Id);

        if (targetHobby == null)
        {
            throw new InvalidOperationException("The hobby was not found.");
        }

        _mapper.Map(source: hobby, destination: targetHobby);
    }

    private int GetNextHobbyId()
    {
        // Simulate database IDs for now
        return _hobbies.Max(x => x.Id) + 1;
    }
}
