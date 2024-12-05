namespace HobbyManagement.Controls.HobbyList.ViewModels;

/// <summary>
/// Design time view model for hobbies. 
/// </summary>
public class DesignHobbyViewModel
{
    #region Constructors
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">The name of the hobby.</param>
    /// <param name="description">The description of the hobby.</param>
    public DesignHobbyViewModel(string name, string description)
    {
        Name = name;
        Description = description;
    }

    #endregion

    #region Properties    

    /// <summary>
    /// The description of the hobby.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// The description of the hobby when in edit mode. 
    /// </summary>
    public string EditDescription { get; set; } = "";

    /// <summary>
    /// The name of the hobby when in edit mode. 
    /// </summary>
    public string EditName { get; set; } = "";

    /// <summary>
    /// Returns true if the edit mode is enabled. 
    /// </summary>
    public bool IsEditing { get => false; }
    
    /// <summary>
    /// The name of the hobby.
    /// </summary>
    public string Name { get; set; } = "";

    #endregion
}
