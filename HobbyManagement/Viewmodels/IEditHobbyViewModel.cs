namespace HobbyManagement.Viewmodels;

public interface IEditHobbyViewModel
{
    string EditDescription { get; set; }
    string EditName { get; set; }
    public int Id { get; set;  }
}