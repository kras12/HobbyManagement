using HobbyManagment.Data;

namespace HobbyManagement.Viewmodels;

public interface IHobbyViewModel
{
    string Description { get; set; }
    string EditDescription { get; set; }
    string EditName { get; set; }
    bool IsEditing { get; set; }
    string Name { get; set; }
    Hobby GetWrappedHobby();
    bool IsEmpty();
    void SetWrappedHobby(Hobby hobby);
    void StartEdit();
}