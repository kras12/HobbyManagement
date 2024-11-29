using HobbyManagment.Data;
using System.Windows.Input;

namespace HobbyManagement.Viewmodels;

public interface IHobbyViewModel
{
    event EventHandler<HobbyViewModel> OnCancelEditHobby;
    event EventHandler<HobbyViewModel> OnSaveEditHobby;
    ICommand CancelEditHobbyCommand { get; }
    string Description { get; set; }
    string EditDescription { get; set; }
    string EditName { get; set; }
    bool IsEditing { get; set; }
    string Name { get; set; }
    ICommand SaveHobbyCommand { get; }
    ICommand StartEditHobbyCommand { get; }
    Hobby GetWrappedHobby();
    bool IsEmpty();
    void SetWrappedHobby(Hobby hobby);
    void StartEdit();
}