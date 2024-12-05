using HobbyManagment.Data.Database;
using HobbyManagment.Data.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace HobbyManagment.Data.Repositories;

public class HobbiesRepository : IHobbiesRepository
{
    private readonly ApplicationDbContext _context;

    public HobbiesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HobbyEntity> CreateHobbyAsync(HobbyEntity hobby)
    {
        _context.Add(hobby);
        await _context.SaveChangesAsync();
        _context.Entry(hobby).State = EntityState.Detached;
        return hobby;
    }

    public async Task DeleteHobbyAsync(HobbyEntity hobby)
    {
        _context.Remove(hobby);
        await _context.SaveChangesAsync();
    }

    public async Task<List<HobbyEntity>> GetAllAsync()
    {
        return await _context.Hobbies.AsNoTracking().ToListAsync();
    }

    public async Task<HobbyEntity?> GetAsync(int id)
    {
        return await _context.Hobbies.AsNoTracking().FirstOrDefaultAsync(x => x.HobbyId == id);
    }

    public async Task<bool> HobbyExists(string name)
    {
        return await _context.Hobbies.AnyAsync(x => x.Name == name);
    }

    public async Task<HobbyEntity> Update(HobbyEntity hobby)
    {
        _context.Update(hobby);
        await _context.SaveChangesAsync();
        _context.Entry(hobby).State = EntityState.Detached;
        return hobby;
    }
}
