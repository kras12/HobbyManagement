using AutoMapper;
using HobbyManagment.Data;
using HobbyManagment.Shared;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Threading;

namespace HobbyManagement.Viewmodels;

public class HobbyViewModel : ObservableObjectBase, IHobbyViewModel
{
    #region Fields

    private readonly IMapper _mapper;
    private bool _isEditing;
    private bool _isUpdated;
    private Hobby _wrappedHobby = default!;
    private IEditHobbyViewModel? editHobbyData;

    #endregion

    #region Constructors

    public HobbyViewModel(IMapper mapper)
    {
        _mapper = mapper;
        SetWrappedHobby(new Hobby() { Name = "", Description = ""});
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
    }

    public IEditHobbyViewModel? EditHobbyData
    {
        get
        {
            return editHobbyData;
        }

        private set
        {
            editHobbyData = value;
            RaisePropertyChanged(nameof(EditHobbyData));
        }
    }

    public int Id => WrappedHobby.Id;

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

    public bool IsUpdated
    {
        get
        { 
            return _isUpdated;
        }

        private set
        {
            _isUpdated = value;
            RaisePropertyChanged(nameof(IsUpdated));
        }
    }

    [Required]
    public string Name
    {
        get
        {
            return WrappedHobby.Name;
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
            && EditHobbyData != null
            && !string.IsNullOrEmpty(EditHobbyData.EditName)
            && !string.IsNullOrEmpty(EditHobbyData.EditDescription);
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

    public Hobby SaveEdit()
    {
        if (!IsEditing || EditHobbyData == null)
        {
            throw new InvalidOperationException("Can't save edit when not editing.");
        }

        var result = _mapper.Map<IEditHobbyViewModel, Hobby>(EditHobbyData);
        EndEdit();
        SetAsUpdated();

        return result;
    }

    public void SetAsUpdated()
    {
        IsUpdated = true;
        var timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2) };
        timer.Tick += (sender, e) =>
        {
            IsUpdated = false;
            timer.Stop();
        };
        timer.Start();
    }

    public void SetWrappedHobby(Hobby hobby)
    {
        WrappedHobby = hobby ?? throw new ArgumentNullException(nameof(hobby));
    }

    public void StartEdit()
    {
        EditHobbyData = _mapper.Map<IHobbyViewModel, IEditHobbyViewModel>(this);
        IsEditing = true;
    }

    private void EndEdit()
    {
        EditHobbyData = null;
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

            case nameof(Hobby.Id):
                RaisePropertyChanged(nameof(Id));
                break;
        }
    }

    #endregion
}
