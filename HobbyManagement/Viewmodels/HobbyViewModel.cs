using HobbyManagment.Data;
using HobbyManagment.Shared;

namespace HobbyManagement.Viewmodels;

public class HobbyViewModel : ObservableObjectBase
{
    private Hobby _hobby;

    public HobbyViewModel(Hobby hobby)
    {
        _hobby = hobby;
        _hobby.PropertyChanged += HobbyPropertyChangedEventHandler;
    }

    public string Description
    {
        get
        {
            return _hobby.Description;
        }

        set
        {
            _hobby.Description = value;
        }
    }

    public string Name
    {
        get
        {
            return _hobby.Name;
        }

        set
        {
            _hobby.Name = value;
        }
    }

    public Hobby GetHobby()
    {
        return _hobby;
    }

    private void HobbyPropertyChangedEventHandler(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Hobby.Name):
                RaisePropertyChanged(nameof(Name));
                break;

            case nameof(Hobby.Description):
                RaisePropertyChanged(nameof(Description));
                break;
        }
    }
}
