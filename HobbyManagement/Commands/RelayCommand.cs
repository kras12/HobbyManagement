using System.Windows.Input;

namespace HobbyManagement.Commands;

/// <summary>
/// Relay command class to handle commands without parameters.
/// </summary>
public class RelayCommand : ICommand
{
    #region Fields

    /// <summary>
    /// Delegate for a function that returns true if the command can be executed. 
    /// </summary>
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// Delegate for a parameterless action that executes the command.
    /// </summary>
    private readonly Action? _execute;

    /// <summary>
    /// Delegate for a parameterless async function that executes the command.
    /// </summary>
    private readonly Func<Task>? _executeTask;

    /// <summary>
    /// True while a command is executing. 
    /// </summary>
    private bool _isExecuting;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="execute">Delegate for a parameterless action that executes the command.</param>
    /// <param name="canExecute">Delegate for a function that returns true if the command can be executed. </param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="executeTask">Delegate for a parameterless async function that executes the command.</param>
    /// <param name="canExecute">Delegate for a function that returns true if the command can be executed. </param>
    public RelayCommand(Func<Task> executeTask, Func<bool>? canExecute = null)
    {
        _executeTask = executeTask;
        _canExecute = canExecute;
    }

    #endregion

    #region Events

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    #endregion

    #region Methods

    /// <inheritdoc/>
    public bool CanExecute(object? input)
    {
        if (_isExecuting)
        {
            return false;
        }

        return _canExecute == null || _canExecute();
    }

    /// <inheritdoc/>
    public async void Execute(object? input)
    {
        try
        {
            _isExecuting = true;

            if (_executeTask != null)
            {
                await _executeTask();
            }
            else if (_execute != null)
            {
                _execute();
            }
        }
        finally
        {
            _isExecuting = false;
        }
        
    }

    #endregion
}
