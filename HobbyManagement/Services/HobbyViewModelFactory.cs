using HobbyManagement.Viewmodels;
using Microsoft.Extensions.DependencyInjection;

namespace HobbyManagement.Services;

/// <summary>
/// A factory service to create <see cref="IHobbyViewModel"/> objects.
/// </summary>
public class HobbyViewModelFactory : IHobbyViewModelFactory
{
    #region Fields

    /// <summary>
    /// Injected service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="serviceProvider">Injected service provider.</param>
    public HobbyViewModelFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates a <see cref="IHobbyViewModel"/> object.
    /// </summary>
    /// <returns><see cref="IHobbyViewModel"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IHobbyViewModel CreateHobbyViewModel()
    {
        return _serviceProvider.GetService<IHobbyViewModel>() as IHobbyViewModel ?? throw new InvalidOperationException("Failed to create a hobby view model");
    }

    #endregion
}
