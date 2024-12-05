using HobbyManagment.Data.Database.Models;

namespace HobbyManagment.Data.Repositories;
public interface IHobbiesRepository
{
    Task<HobbyEntity> CreateHobbyAsync(HobbyEntity hobby);
    Task DeleteHobbyAsync(HobbyEntity hobby);
    Task<List<HobbyEntity>> GetAllAsync();
    Task<HobbyEntity?> GetAsync(int id);
    Task<bool> HobbyExists(string name);
    Task<HobbyEntity> Update(HobbyEntity hobby);
}