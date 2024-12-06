using AutoMapper;
using HobbyManagement.Business;
using HobbyManagment.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Windows.Threading;

namespace HobbyManagement.Viewmodels;

/// <summary>
/// View model for hobby.
/// </summary>
public class HobbyViewModel : ObservableObjectBase, IHobbyViewModel
{
    #region Fields

    /// <summary>
    /// Injected data mapper.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Injected service provider. 
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Backing field for property <see cref="IsEditing"/>.
    /// </summary>
    private bool _isEditing;

    /// <summary>
    /// Backing field for property <see cref="IsUpdated"/>.
    /// </summary>
    private bool _isUpdated;

    /// <summary>
    /// The wrapped <see cref="IHobby"/>.
    /// </summary>
    private IHobby _wrappedHobby = default!;

    /// <summary>
    /// Backing field for property <see cref="EditHobbyData"/>.
    /// </summary>
    private IEditHobbyViewModel? editHobbyData;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mapper">Injected data mapper.</param>
    /// <param name="serviceProvider">Injected service provider.</param>
    public HobbyViewModel(IMapper mapper, IServiceProvider serviceProvider)
    {
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        SetWrappedHobby(_serviceProvider.GetRequiredService<IHobby>());
    }

    #endregion

    #region Properties

    /// <summary>
    /// The description of the hobby.
    /// </summary>
    [Required]
    public string Description
    {
        get
        {
            return WrappedHobby.Description;
        }
    }

    /// <summary>
    /// Temporarily stores the new hobby data while editing a hobby. 
    /// </summary>
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

    /// <summary>
    /// The ID of the hobby.
    /// </summary>
    public int Id => WrappedHobby.Id;

    /// <summary>
    /// Returns true if the hobby is currently being edited. 
    /// </summary>
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

    /// <summary>
    /// Returns true if the hobby was recently updated with new data.
    /// </summary>
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

    /// <summary>
    /// The name of the hobby.
    /// </summary>
    [Required]
    public string Name
    {
        get
        {
            return WrappedHobby.Name;
        }
    }

    /// <summary>
    /// The underlying <see cref="IHobby"/> that is being wrapped. 
    /// </summary>
    private IHobby WrappedHobby
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

    /// <summary>
    /// Cancels the edit mode. 
    /// </summary>
    public void CancelEdit()
    {
        EndEdit();
    }

    /// <summary>
    /// Returns true if the hobby is in edit mode and have new valid data that can be saved. 
    /// </summary>
    /// <returns></returns>
    public bool CanSave()
    {
        // TODO - Implement better validation and messaging methods. 
        return IsEditing
            && EditHobbyData != null
            && !string.IsNullOrEmpty(EditHobbyData.EditName)
            && !string.IsNullOrEmpty(EditHobbyData.EditDescription);
    }

    /// <summary>
    /// Returns a csv string representation of the hobby data. 
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string HobbyAsCSV()
    {
        return @$"""{Name}"", ""{Description}""";
    }

    /// <summary>
    /// Returns a csv string containing the attributes of a hobby.
    /// </summary>
    /// <remarks>Names are enclosed in quotes.</remarks>
    /// <returns><see cref="string"/></returns>
    public string HobbyAttributesAsCsvHeader()
    {
        string quoteString = @"""";
        StringBuilder stringBuilder = new();

        foreach (var headerName in HobbyAttributesAsHeaderList())
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(",");
            }

            stringBuilder.Append($"{quoteString}{headerName}{quoteString}");
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns a collection of header names that describes the attributes of a hobby.
    /// </summary>
    /// <returns>A collection of <see cref="string"/>.</returns>
    public List<string> HobbyAttributesAsHeaderList()
    {
        return new List<string>()
        {
            $"{nameof(Name)}",
            $"{nameof(Description)}"
        };
    }

    /// <summary>
    /// Returns true if the required attributes of a hobby is not set. 
    /// </summary>
    /// <returns><see cref="bool"/></returns>
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

    /// <summary>
    /// Returns a new instance of <see cref="IHobby>"/> containing the new hobby attributes provided in the edit mode. 
    /// </summary>
    /// <returns><see cref="IHobby"/></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IHobby SaveEdit()
    {
        if (!IsEditing || EditHobbyData == null)
        {
            throw new InvalidOperationException("Can't save edit when not editing.");
        }

        var result = _mapper.Map<IEditHobbyViewModel, IHobby>(EditHobbyData);
        EndEdit();
        SetAsUpdated();

        return result;
    }

    /// <summary>
    /// Temporarily marks this hobby as having new attribute values. 
    /// </summary>
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

    /// <summary>
    /// Sets the <see cref="IHobby"/> that is being wrapped. 
    /// </summary>
    /// <param name="hobby">The hobby to wrap.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SetWrappedHobby(IHobby hobby)
    {
        WrappedHobby = hobby ?? throw new ArgumentNullException(nameof(hobby));
    }

    /// <summary>
    /// Starts the editing mode. 
    /// </summary>
    public void StartEdit()
    {
        EditHobbyData = _mapper.Map<IHobbyViewModel, IEditHobbyViewModel>(this);
        IsEditing = true;
    }

    /// <summary>
    /// Ends the editing mode.
    /// </summary>
    private void EndEdit()
    {
        EditHobbyData = null;
        IsEditing = false;
    }

    /// <summary>
    /// Event handler for property changes in the wrapped <see cref="IHobby"/>.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event parameter containing the changes.</param>
    private void HobbyPropertyChangedEventHandler(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IHobby.Name):
                RaisePropertyChanged(nameof(Name));
                break;

            case nameof(IHobby.Description):
                RaisePropertyChanged(nameof(Description));
                break;

            case nameof(IHobby.Id):
                RaisePropertyChanged(nameof(Id));
                break;
        }
    }

    #endregion
}
