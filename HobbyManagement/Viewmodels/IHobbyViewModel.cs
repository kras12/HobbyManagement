using HobbyManagement.Business;

namespace HobbyManagement.Viewmodels;

public interface IHobbyViewModel
{
    /// <summary>
    /// The description of the hobby.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Temporarily stores the new hobby data while editing a hobby. 
    /// </summary>
    IEditHobbyViewModel? EditHobbyData { get; }

    /// <summary>
    /// The ID of the hobby.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Returns true if the hobby is currently being edited. 
    /// </summary>
    bool IsEditing { get; set; }

    /// <summary>
    /// Returns true if the hobby was recently updated with new data.
    /// </summary>
    bool IsUpdated { get; }

    /// <summary>
    /// The name of the hobby.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns a csv string representation of the hobby data. 
    /// </summary>
    /// <returns><see cref="string"/></returns>
    string HobbyAsCSV();

    /// <summary>
    /// Returns a csv string containing the attributes of a hobby.
    /// </summary>
    /// <remarks>Names are enclosed in quotes.</remarks>
    /// <returns><see cref="string"/></returns>
    string HobbyAttributesAsCsvHeader();

    /// <summary>
    /// Returns a collection of header names that describes the attributes of a hobby.
    /// </summary>
    /// <returns>A collection of <see cref="string"/>.</returns>
    List<string> HobbyAttributesAsHeaderList();

    /// <summary>
    /// Returns true if the required attributes of a hobby is not set. 
    /// </summary>
    /// <returns><see cref="bool"/></returns>
    bool IsEmpty();

    /// <summary>
    /// Returns a new instance of <see cref="IHobby>"/> containing the new hobby attributes provided in the edit mode. 
    /// </summary>
    /// <returns><see cref="IHobby"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    IHobby SaveEdit();

    /// <summary>
    /// Temporarily marks this hobby as having new attribute values. 
    /// </summary>
    public void SetAsUpdated();

    /// <summary>
    /// Sets the <see cref="IHobby"/> that is being wrapped. 
    /// </summary>
    /// <param name="hobby">The hobby to wrap.</param>
    /// <exception cref="ArgumentNullException"></exception>
    void SetWrappedHobby(IHobby hobby);

    /// <summary>
    /// Starts the editing mode. 
    /// </summary>
    void StartEdit();
}