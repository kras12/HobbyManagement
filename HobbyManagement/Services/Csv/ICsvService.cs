using HobbyManagement.Services.Csv.Data;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagement.Services.Csv;

/// <summary>
/// Interface for a service to read and write CSV content to and from disk.
/// </summary>
public interface ICsvService
{
    /// <summary>
    /// Attempts to read the content from a csv file on disk. The user is presented with a open file dialog. 
    /// </summary>
    /// <param name="csvFile">The csv content in the form of a <see cref="CsvFile"/> if the operation was succesful.</param>
    /// <returns>True if the operation was successful. False if the user did not chose a file.</returns>
    bool TryReadCsvFile([NotNullWhen(true)] out CsvFile? csvFile);

    /// <summary>
    /// Attempts to write csv content to a file on disk. 
    /// </summary>
    /// <param name="csvContent">The rows to write to the file.</param>
    /// <returns>True if the operation was successful.</returns>
    bool TryWriteCsvFile(Func<List<string>> csvContent);
}