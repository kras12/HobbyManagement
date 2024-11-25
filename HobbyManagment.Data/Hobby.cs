using HobbyManagment.Shared;

namespace HobbyManagment.Data;

public class Hobby : ObservableObjectBase
{
    private string name = "";
    private string description = "";

    public Hobby(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name
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

    public string Description
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
}
