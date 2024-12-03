using HobbyManagment.Data;

namespace HobbyManagement.Viewmodels;

public interface IHobbyViewModel
{
    string Description { get; }
    IEditHobbyViewModel? EditHobbyData { get; }
    int Id { get; }
    bool IsEditing { get; set; }
    string Name { get; }
    string HobbyAsCSV();
    string HobbyHeaderAsCSV();
    bool IsEmpty();
    public void SetAsUpdated();
    void SetWrappedHobby(Hobby hobby);
    void StartEdit();
}