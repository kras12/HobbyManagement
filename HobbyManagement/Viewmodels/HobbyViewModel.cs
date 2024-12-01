using HobbyManagment.Data;
using HobbyManagment.Shared;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HobbyManagement.Viewmodels;

public class HobbyViewModel : ObservableObjectBase, IHobbyViewModel
{
    #region Fields

    private string _editDescription = "";
    private string _editName = "";
    private bool _isEditing;
    private Hobby _wrappedHobby = default!;

    #endregion

    #region Constructors

    public HobbyViewModel()
    {
        SetWrappedHobby(new Hobby() { Name = ""});
    }

    #endregion

    #region Properties

    [Required]
    public string Description
    {
        get
        {
            return WrappedHobby.Description;
        }

        set
        {
            WrappedHobby.Description = value;
        }
    }

    public string EditDescription
    {
        get
        {
            return _editDescription;
        }

        set
        {
            _editDescription = value;
            RaisePropertyChanged(nameof(EditDescription));
        }
    }

    public string EditName
    {
        get
        {
            return _editName;
        }

        set
        {
            _editName = value;
            RaisePropertyChanged(nameof(EditName));
        }
    }

    public bool IsEditing
    {
        get
        {
            return _isEditing;
        }

        set
        {
            _isEditing = value;
            RaisePropertyChanged(nameof(IsEditing));
        }
    }

    [Required]
    public string Name
    {
        get
        {
            return WrappedHobby.Name;
        }

        set
        {
            WrappedHobby.Name = value;
        }
    }

    private Hobby WrappedHobby
    {
        get
        {
            return _wrappedHobby;
        }

        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(WrappedHobby));
            }

            if (_wrappedHobby != null)
            { 
                _wrappedHobby.PropertyChanged -= HobbyPropertyChangedEventHandler;
            }

            _wrappedHobby = value;
            _wrappedHobby.PropertyChanged += HobbyPropertyChangedEventHandler;
        }
    }

    #endregion

    #region Methods

    public void CancelEdit()
    {
        EndEdit();
    }

    public bool CanSave()
    {
        // TODO - Implement better validation and messaging methods. 
        return IsEditing
            && !string.IsNullOrEmpty(EditName)
            && !string.IsNullOrEmpty(EditDescription);
    }

    public Hobby GetWrappedHobby()
    {
        return WrappedHobby;
    }

    public bool IsEmpty()
    {
        foreach (PropertyInfo property in typeof(HobbyViewModel).GetProperties())
        {
            if (Attribute.IsDefined(property, typeof(RequiredAttribute)))
            {
                var value = property.GetValue(this);
                var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;

                if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrEmpty(value as string))
                    {
                        return false;
                    }
                }
                else if (!Equals(value, defaultValue))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SaveEdit()
    {
        ReplaceValuesWithEditValues();
        EndEdit();
    }

    public void SetWrappedHobby(Hobby hobby)
    {
        WrappedHobby = hobby ?? throw new ArgumentNullException(nameof(hobby));
    }

    public void StartEdit()
    {
        InitEditValues();
        IsEditing = true;
    }

    private void EndEdit()
    {
        EditName = "";
        EditDescription = "";
        IsEditing = false;
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

    private void InitEditValues()
    {
        EditName = Name;
        EditDescription = Description;
    }

    private void ReplaceValuesWithEditValues()
    {
        Name = EditName;
        Description = EditDescription;
    }

    #endregion
}
