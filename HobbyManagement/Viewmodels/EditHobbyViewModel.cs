using HobbyManagment.Shared;
namespace HobbyManagement.Viewmodels;

/// <summary>
/// View model that contains edit properties for a hobby. 
/// </summary>
public class EditHobbyViewModel : ObservableObjectBase, IEditHobbyViewModel
{
    #region Fields

    /// <summary>
    /// Backing field for property <see cref="EditDescription"/>.
    /// </summary>
    private string _editDescription = "";

    /// <summary>
    /// Backing field for property <see cref="EditName"/>.
    /// </summary
    private string _editName = "";

    #endregion

    #region Properties

    /// <summary>
    /// The description of the hobby in edit mode. 
    /// </summary>
    public string EditDescription
    {
        get
        {
            return _editDescription;
        }

        set
        {
            _editDescription = value;
            RaisePropertyChanged(nameof(EditDescription));
        }
    }

    /// <summary>
    /// The name of the hobby in edit mode. 
    /// </summary>
    public string EditName
    {
        get
        {
            return _editName;
        }

        set
        {
            _editName = value;
            RaisePropertyChanged(nameof(EditName));
        }
    }

    /// <summary>
    /// The ID of the hobby in edit mode. 
    /// </summary>
    public int Id { get; set;  }

    #endregion
}
