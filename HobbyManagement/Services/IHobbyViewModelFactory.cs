using HobbyManagement.Viewmodels;

namespace HobbyManagement.Services;

/// <summary>
/// Interface for a factory service to create <see cref="IHobbyViewModel"/> objects.
/// </summary>
public interface IHobbyViewModelFactory
{
    /// <summary>
    /// Creates a <see cref="IHobbyViewModel"/> object.
    /// </summary>
    /// <returns><see cref="IHobbyViewModel"/></returns>
    IHobbyViewModel CreateHobbyViewModel();
}