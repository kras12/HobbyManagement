using AutoMapper;
using HobbyManagment.Data.Database.Models;
using HobbyManagment.Data.Repositories;
using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HobbyManagement.Business;

/// <summary>
/// Handles business logic for hobbies. 
/// </summary>
public class HobbyManager : ObservableObjectBase, IHobbyManager
{
    #region Fields
    
    /// <summary>
    /// A collection of hobbies.
    /// </summary>
    private readonly ObservableCollection<Hobby> _hobbies = [];

    /// <summary>
    /// The injected repository for hobbies.
    /// </summary>
    private readonly IHobbiesRepository _hobbiesRepository;

    /// <summary>
    /// The injected mapper service. 
    /// </summary>
    private readonly IMapper _mapper;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor. 
    /// </summary>
    /// <param name="hobbiesRepository">The injected repository for hobbies.</param>
    /// <param name="mapper">The injected mapper service. </param>
    public HobbyManager(IHobbiesRepository hobbiesRepository, IMapper mapper)
    {
        Hobbies = new ReadOnlyObservableCollection<Hobby>(_hobbies);
        _hobbiesRepository = hobbiesRepository;
        _mapper = mapper;
    }

    #endregion

    #region Events

    /// <summary>
    /// Collection changed event for hobbies.
    /// </summary>
    public event NotifyCollectionChangedEventHandler HobbiesChanged
    {
        add { _hobbies.CollectionChanged += value; }
        remove { _hobbies.CollectionChanged -= value; }
    }

    #endregion

    #region Properties

    /// <summary>
    /// A readonly and observable collection of hobbies. 
    /// </summary>
    public ReadOnlyObservableCollection<Hobby> Hobbies { get; }

    #endregion

    #region Methods
        
    /// <summary>
    /// Creates a new hobby in the database and adds it to the collection.
    /// </summary>
    /// <param name="hobby">The hobby to add.</param>
    /// <returns><see cref="Task"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// Deletes a hobby from the database and removes it from the collection. 
    /// </summary>
    /// <param name="Id">The ID of the hobby.</param>
    /// <returns><see cref="Task"/></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// Checks whether a hobby exists in the collection.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="excludeHobbyId">An optional ID of a hobby to ignore.</param>
    /// <returns></returns>
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

    /// <summary>
    /// A task that loads the data of the manager. 
    /// This includes fetching hobbies from the database.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    public async Task LoadDataAsync()
    {
        try
        {
            var hobbies = await _hobbiesRepository.GetAllAsync();

            foreach (var hobby in hobbies)
            {
                _hobbies.Add(_mapper.Map<Hobby>(hobby));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while loading hobbies: {ex.Message}");
            throw;
        }
        
    }

    /// <summary>
    /// Updates a hobby in the database and in the collection. 
    /// </summary>
    /// <param name="hobby">The hobby to update.</param>
    /// <returns><see cref="Task"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
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

    #endregion
}
