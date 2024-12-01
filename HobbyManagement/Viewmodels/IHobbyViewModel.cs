using HobbyManagment.Data;

namespace HobbyManagement.Viewmodels;

public interface IHobbyViewModel
{
    string Description { get; }
    IEditHobbyViewModel? EditHobbyData { get; }
    int Id { get; }
    bool IsEditing { get; set; }
    string Name { get; }
    bool IsEmpty();
    void SetWrappedHobby(Hobby hobby);
    void StartEdit();
}