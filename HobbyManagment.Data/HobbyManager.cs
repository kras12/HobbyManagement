using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagment.Data;

public class HobbyManager : ObservableObjectBase
{
    private readonly ObservableCollection<Hobby> _hobbies = [];

    public HobbyManager()
    {
        Hobbies = new ReadOnlyObservableCollection<Hobby>(_hobbies);
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

        _hobbies.Add(hobby);
    }

    public void DeleteHobby(Hobby hobby)
    {
        _hobbies.Remove(hobby);
    }

    public void ClearHobbies()
    {
        _hobbies.Clear();
    }

    public bool HobbyExists(string name)
    {
        return _hobbies.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public Task LoadData()
    {
        _hobbies.Add(new Hobby(id: 1, name: "Weight Training", description: "Weight training at the gym."));        
        _hobbies.Add(new Hobby(id: 2, name: "Movies and TV-series", description: "Occasionally watching movies and TV-series."));
        _hobbies.Add(new Hobby(id: 3, name: "Programming", description: "Programming with C# .Net."));

        return Task.CompletedTask;
    }

    public void RemoveHobby(Hobby hobby)
    {
        if (!_hobbies.Contains(hobby))
        {
            throw new InvalidOperationException("The hobby was not found.");
        }

        _hobbies.Remove(hobby);
    }
}
