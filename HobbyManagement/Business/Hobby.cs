using HobbyManagment.Shared;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagement.Business;

/// <summary>
/// A class that represents a hobby and handles business logic.
/// </summary>
public class Hobby : ObservableObjectBase, IHobby
{
    #region Fields

    /// <summary>
    /// Backing field for property <see cref="Description"/>.
    /// </summary>
    private string description = "";

    /// <summary>
    /// Backing field for property <see cref="Name"/>.
    /// </summary>
    private string name = "";

    /// <summary>
    /// Backing field for property <see cref="Id"/>.
    /// </summary>
    private int id;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public Hobby()
    {

    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="description">The description of the hobby.</param>
    [SetsRequiredMembers]
    public Hobby(string name, string description) : this(id: 0, name: name, description: description)
    {

    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="id">The ID of the hobby.</param>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="description">The description of the hobby.</param>
    [SetsRequiredMembers]
    public Hobby(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The description of the hobby.
    /// </summary>
    public required string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
            RaisePropertyChanged(nameof(Description));
        }
    }

    /// <summary>
    /// The ID of the hobby.
    /// </summary>
    public int Id
    {
        get
        {
            return id;
        }


        set
        {
            id = value;
            RaisePropertyChanged(nameof(Id));
        }
    }

    /// <summary>
    /// The name of the hobby.
    /// </summary>
    public required string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
            RaisePropertyChanged(nameof(Name));
        }
    }

    #endregion
}
