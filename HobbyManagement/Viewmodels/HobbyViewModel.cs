using HobbyManagement.Commands;
using HobbyManagment.Data;
using HobbyManagment.Shared;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;

namespace HobbyManagement.Viewmodels;

public class HobbyViewModel : ObservableObjectBase
{
    #region Fields

    private string _editDescription;
    private string _editName;
    private bool _isEditing;
    private Hobby _wrappedHobby;
    #endregion

    #region Constructors

    public HobbyViewModel(Hobby hobby)
    {
        _wrappedHobby = hobby;
        _wrappedHobby.PropertyChanged += HobbyPropertyChangedEventHandler;

        SaveHobbyCommand = new GenericRelayCommand<HobbyViewModel>(SaveEdit, CanSave);
        StartEditHobbyCommand = new GenericRelayCommand<HobbyViewModel>(StartEdit, CanStartEdit);
        CancelEditHobbyCommand = new GenericRelayCommand<HobbyViewModel>(CancelEdit, CanCancelEdit);
    }

    #endregion

    #region Events

    public event EventHandler<HobbyViewModel> OnCancelEditHobby;
    public event EventHandler<HobbyViewModel> OnSaveEditHobby;

    #endregion

    #region Properties

    [Required]
    public string Description
    {
        get
        {
            return _wrappedHobby.Description;
        }

        set
        {
            _wrappedHobby.Description = value;
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
            return _wrappedHobby.Name;
        }

        set
        {
            _wrappedHobby.Name = value;
        }
    }

    private bool CanCancelEditing => IsEditing;
    private bool CanStartEditing => !IsEditing;

    #endregion

    #region Commands

    public ICommand CancelEditHobbyCommand { get; }
    public ICommand SaveHobbyCommand { get; }
    public ICommand StartEditHobbyCommand { get; }

    #endregion

    #region CommandMethods

    private static bool CanCancelEdit(HobbyViewModel hobby)
    {
        return hobby.CanCancelEditing;
    }

    private static void CancelEdit(HobbyViewModel hobby)
    {
        if (CanCancelEdit(hobby))
        {
            hobby.EndEditMode();
            hobby.OnCancelEditHobby?.Invoke(hobby, hobby);
        }
    }

    private static bool CanSave(HobbyViewModel hobby)
    {
        // TODO - Implement better validation and messaging methods. 
        return hobby.IsEditing
            && !string.IsNullOrEmpty(hobby.EditName)
            && !string.IsNullOrEmpty(hobby.EditDescription);
    }

    private static bool CanStartEdit(HobbyViewModel hobby)
    {
        return hobby.CanStartEditing;
    }

    private static void SaveEdit(HobbyViewModel hobby)
    {
        if (CanSave(hobby))
        {
            hobby.ReplaceValuesWithEditValues();
            hobby.EndEditMode();
            hobby.OnSaveEditHobby?.Invoke(hobby, hobby);
        }
    }

    private static void StartEdit(HobbyViewModel hobby)
    {
        if (hobby.CanStartEditing)
        {
            hobby.StartEdit();
        }
    }

    #endregion

    #region Methods

    public Hobby GetWrappedHobby()
    {
        return _wrappedHobby;
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

    public void StartEdit()
    {
        InitEditValues();
        IsEditing = true;
    }

    private void EndEditMode()
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
