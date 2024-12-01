using HobbyManagment.Shared;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagment.Data;

public class Hobby : ObservableObjectBase
{
    private string name = "";
    private string description = "";

    public Hobby()
    {
        
    }

    [SetsRequiredMembers]
    public Hobby(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
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

    public int Id { get; private init; }
}
