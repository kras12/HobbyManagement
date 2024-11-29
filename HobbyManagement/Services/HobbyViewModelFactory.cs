using HobbyManagement.Viewmodels;
using Microsoft.Extensions.DependencyInjection;

namespace HobbyManagement.Services;

public class HobbyViewModelFactory : IHobbyViewModelFactory
{
    private readonly IServiceProvider _serviceProvider;

    public HobbyViewModelFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IHobbyViewModel CreateHobbyViewModel()
    {
        return _serviceProvider.GetService<IHobbyViewModel>() as IHobbyViewModel ?? throw new InvalidOperationException("Failed to create a hobby view model");
    }
}
