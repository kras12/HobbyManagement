namespace HobbyManagement.Services.Csv.Error;

/// <summary>
/// Is thrown when an invalid format was detected in csv content. 
/// </summary>
public class InvalidCsvFormatException : Exception
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidCsvFormatException(string? message) : base(message)
    {

    }
}
