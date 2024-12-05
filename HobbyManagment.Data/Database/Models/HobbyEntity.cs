using System.Diagnostics.CodeAnalysis;

namespace HobbyManagment.Data.Database.Models;

public class HobbyEntity
{
    public HobbyEntity()
    {

    }

    [SetsRequiredMembers]
    public HobbyEntity(string name, string description) : this(id: 0, name: name, description: description)
    {

    }

    [SetsRequiredMembers]
    public HobbyEntity(int id, string name, string description)
    {
        HobbyId = id;
        Name = name;
        Description = description;
    }

    public required string Description { get; set; } = "";

    public int HobbyId { get; set; }

    public required string Name { get; set; } = "";
}
