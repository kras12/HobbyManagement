namespace HobbyManagement.Services.Csv.Error;

public class InvalidCsvFormatException : Exception
{
    public InvalidCsvFormatException(string? message) : base(message)
    {

    }
}
