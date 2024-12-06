namespace HobbyManagement.Viewmodels;

public interface IEditHobbyViewModel
{
    /// <summary>
    /// The description of the hobby in edit mode. 
    /// </summary>
    string EditDescription { get; set; }

    /// <summary>
    /// The name of the hobby in edit mode. 
    /// </summary>
    string EditName { get; set; }

    /// <summary>
    /// The ID of the hobby in edit mode. 
    /// </summary>
    public int Id { get; set;  }
}