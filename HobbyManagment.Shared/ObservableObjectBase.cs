using System.ComponentModel;

namespace HobbyManagment.Shared;

/// <summary>
/// Base class that implements support for INotifyPropertyChanged.
/// </summary>
public class ObservableObjectBase : INotifyPropertyChanged
{
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the property changed event. 
    /// </summary>
    /// <param name="propertyName"></param>
    protected void RaisePropertyChanged(string propertyName)
    {
        // Design choice:
        // Attribute CallerMemberName is not used here since often in WPF applications you raise the event on multiple properties in one setter. 
        // Having one call without an argument leads to an inconsistency that can lead to confusion. 
        // Performing searches in a project for all places where the event is raised for a particular property is also easier if the method argument is required. 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
