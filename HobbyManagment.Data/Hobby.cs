using HobbyManagment.Shared;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagment.Data;

public class Hobby : ObservableObjectBase
{
    private string description = "";
    private string name = "";
    private int id;

    public Hobby()
    {
        
    }

    [SetsRequiredMembers]
    public Hobby(string name, string description) : this(id: 0, name:name, description: description)
    {

    }

    [SetsRequiredMembers]
    public Hobby(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

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
}
