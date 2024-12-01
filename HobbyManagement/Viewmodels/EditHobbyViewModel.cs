using HobbyManagment.Shared;
namespace HobbyManagement.Viewmodels;

public class EditHobbyViewModel : ObservableObjectBase, IEditHobbyViewModel
{
    private string _editDescription = "";
    private string _editName = "";

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

    public int Id { get; set;  }
}
