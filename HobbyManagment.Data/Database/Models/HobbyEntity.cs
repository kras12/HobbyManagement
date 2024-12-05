using System.Diagnostics.CodeAnalysis;

namespace HobbyManagment.Data.Database.Models;

/// <summary>
/// Entity class for hobbies.
/// </summary>
public class HobbyEntity
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public HobbyEntity()
    {

    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="description">The description of the hobby.</param>
    [SetsRequiredMembers]
    public HobbyEntity(string name, string description) : this(id: 0, name: name, description: description)
    {

    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="id">The ID of the hobby.</param>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="description">The description of the hobby.</param>
    [SetsRequiredMembers]
    public HobbyEntity(int id, string name, string description)
    {
        HobbyId = id;
        Name = name;
        Description = description;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The description of the hobby.
    /// </summary>
    public required string Description { get; set; } = "";

    /// <summary>
    /// The ID of the hobby.
    /// </summary>
    public int HobbyId { get; set; }

    /// <summary>
    /// The name of the hobby.
    /// </summary>
    public required string Name { get; set; } = "";

    #endregion
}
