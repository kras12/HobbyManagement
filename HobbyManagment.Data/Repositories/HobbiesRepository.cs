using HobbyManagment.Data.Database;
using HobbyManagment.Data.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace HobbyManagment.Data.Repositories;

/// <summary>
/// Repository class for hobbies.
/// </summary>
public class HobbiesRepository : IHobbiesRepository
{
    #region Fields

    /// <summary>
    /// Injected database context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="context">Injected database context.</param>
    public HobbiesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    
    /// <summary>
    /// Adds a hobby to the database.
    /// </summary>
    /// <param name="hobby">The hobby to add.</param>
    /// <returns>The created <see cref="HobbyEntity"/>.</returns>
    public async Task<HobbyEntity> CreateHobbyAsync(HobbyEntity hobby)
    {
        _context.Add(hobby);
        await _context.SaveChangesAsync();
        _context.Entry(hobby).State = EntityState.Detached;
        return hobby;
    }

    /// <summary>
    /// Deletes a hobby from the database.
    /// </summary>
    /// <param name="hobby">The hobby to delete.</param>
    /// <returns><see cref="Task"/></returns>
    public async Task DeleteHobbyAsync(HobbyEntity hobby)
    {
        _context.Remove(hobby);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Fetches all hobbies from the database.
    /// </summary>
    /// <returns>A collection of <see cref="HobbyEntity"/>.</returns>
    public async Task<List<HobbyEntity>> GetAllAsync()
    {
        return await _context.Hobbies.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Attempts to fetch a hobby by ID.
    /// </summary>
    /// <param name="id">The ID of the hobby.</param>
    /// <returns>The found <see cref="HobbyEntity"/> if the operation was successful.</returns>
    public async Task<HobbyEntity?> GetAsync(int id)
    {
        return await _context.Hobbies.AsNoTracking().FirstOrDefaultAsync(x => x.HobbyId == id);
    }

    /// <summary>
    /// Returns the number of hobbies in the database.
    /// </summary>
    /// <returns>The number of hobbies in the form of an <see cref="int"/>.</returns>
    public Task<int> HobbiesCount()
    {
        return _context.Hobbies.CountAsync();
    }

    /// <summary>
    /// Checks whether a hobby exists in the database.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <returns>True if the hobby exists.</returns>
    public async Task<bool> HobbyExists(string name)
    {
        return await _context.Hobbies.AnyAsync(x => x.Name == name);
    }

    /// <summary>
    /// Updates a hobby in the database.
    /// </summary>
    /// <param name="hobby">The hobby to update.</param>
    /// <returns>The updated <see cref="HobbyEntity"/>.</returns>
    public async Task<HobbyEntity> Update(HobbyEntity hobby)
    {
        _context.Update(hobby);
        await _context.SaveChangesAsync();
        _context.Entry(hobby).State = EntityState.Detached;
        return hobby;
    }

    #endregion
}
