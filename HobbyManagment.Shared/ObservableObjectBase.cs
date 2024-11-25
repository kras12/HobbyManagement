using System.ComponentModel;

namespace HobbyManagment.Shared;

public class ObservableObjectBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged(string propertyName) 
    { 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
