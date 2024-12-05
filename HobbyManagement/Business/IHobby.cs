using System.ComponentModel;

namespace HobbyManagement.Business;

public interface IHobby : INotifyPropertyChanged
{
    string Description { get; set; }
    int Id { get; set; }
    string Name { get; set; }
}