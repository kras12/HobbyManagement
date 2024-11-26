using HobbyManagement.Commands;
using HobbyManagment.Data;
using HobbyManagment.Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace HobbyManagement.Viewmodels;

public class HobbyManagerViewModel : ObservableObjectBase
{
    #region Fields
    
    private readonly ReadOnlyObservableCollection<HobbyViewModel> _hobbies;
    private readonly HobbyManager _hobbyManager = new HobbyManager();
    private readonly ObservableCollection<HobbyViewModel> _underlyingHobbiesCollection;
    private bool isLoadingData;

    #endregion

    #region Constructors    

    public HobbyManagerViewModel()
    {
        _underlyingHobbiesCollection = new ObservableCollection<HobbyViewModel>(_hobbyManager.Hobbies.Select(x => new HobbyViewModel(x)).ToList());
        _hobbies = new ReadOnlyObservableCollection<HobbyViewModel>(_underlyingHobbiesCollection);
        _hobbyManager.HobbiesChanged += HobbiesChangedEventHandler;
        AddHobbyCommand = new RelayCommand(AddHobby, () => true);

        LoadDataAsync();
    }

    #endregion

    #region Properties

    public ReadOnlyObservableCollection<HobbyViewModel> Hobbies
    {
        get
        {
            return _hobbies;
        }

        private init
        {
            _hobbies = value;
            RaisePropertyChanged(nameof(Hobbies));
        }
    }

    public bool IsLoadingData
    {
        get
        {
            return isLoadingData;
        }

        private set
        {
            isLoadingData = value;
            RaisePropertyChanged(nameof(IsLoadingData));
        }
    }

    #endregion

    #region Commands

    public RelayCommand AddHobbyCommand { get; }

    #endregion

    #region CommandMethods 

    private void AddHobby()
    {
        var newHobby = new HobbyViewModel(new Hobby("", ""));
        newHobby.StartEdit();
        _underlyingHobbiesCollection.Add(newHobby);

        newHobby.OnCancelEditHobby += (sender, hobby) =>
        {
            _underlyingHobbiesCollection.Remove(newHobby);
        };

        newHobby.OnSaveEditHobby += (sender, hobby) =>
        {
            _underlyingHobbiesCollection.Remove(newHobby);
            _hobbyManager.AddHobby(newHobby.GetWrappedHobby());
        };
    }

    #endregion

    #region Methods

    private void AddHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            _underlyingHobbiesCollection.Add(new HobbyViewModel(hobby));
        }
    }

    private void ClearHobbies()
    {
        _underlyingHobbiesCollection.Clear();
    }

    private void HobbiesChangedEventHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddHobbies(e.NewItems!.Cast<Hobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Remove:
                RemoveHobbies(e.OldItems!.Cast<Hobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Reset:
                ClearHobbies();
                AddHobbies(_hobbyManager.Hobbies.ToList());
                break;

            case NotifyCollectionChangedAction.Replace:
                RemoveHobbies(e.OldItems!.Cast<Hobby>().ToList());
                AddHobbies(e.NewItems!.Cast<Hobby>().ToList());
                break;

            case NotifyCollectionChangedAction.Move:
                MoveHobby(e.OldStartingIndex, e.NewStartingIndex);
                break;
        }
    }

    private async void LoadDataAsync()
    {
        try
        {
            IsLoadingData = true;
            await _hobbyManager.LoadData();
        }
        catch (Exception ex)
        {
            // TOOD
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsLoadingData = false;
        }
    }

    private void MoveHobby(int oldStartingIndex, int newStartingIndex)
    {
        _underlyingHobbiesCollection.Move(oldStartingIndex, newStartingIndex);
    }

    private void RemoveHobbies(List<Hobby> hobbies)
    {
        foreach (Hobby hobby in hobbies)
        {
            var hobbyToRemove = _underlyingHobbiesCollection.FirstOrDefault(x => x.GetWrappedHobby() == hobby);

            if (hobbyToRemove != null)
            {
                _underlyingHobbiesCollection.Remove(hobbyToRemove);
            }
        }
    }

    #endregion
}
