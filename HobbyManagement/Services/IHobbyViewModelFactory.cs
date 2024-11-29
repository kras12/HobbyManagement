using HobbyManagement.Viewmodels;

namespace HobbyManagement.Services
{
    public interface IHobbyViewModelFactory
    {
        IHobbyViewModel CreateHobbyViewModel();
    }
}