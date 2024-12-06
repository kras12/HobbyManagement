using System.ComponentModel;

namespace HobbyManagement.Business;

/// <summary>
/// Interface for a class that represents a hobby and handles business logic.
/// </summary>
public interface IHobby : INotifyPropertyChanged
{
    /// <summary>
    /// The description of the hobby.
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// The ID of the hobby.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// The name of the hobby.
    /// </summary>
    string Name { get; set; }
}