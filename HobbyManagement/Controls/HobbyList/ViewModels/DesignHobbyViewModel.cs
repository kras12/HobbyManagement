namespace HobbyManagement.Controls.HobbyList.ViewModels;

public class DesignHobbyViewModel
{
    public DesignHobbyViewModel(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Description { get; set; } = "";
    public string EditDescription { get; set; } = "";
    public string EditName { get; set; } = "";
    public bool IsEditing { get => false; }
    public string Name { get; set; } = "";
}
