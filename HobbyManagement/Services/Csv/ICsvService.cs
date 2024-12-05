using HobbyManagement.Services.Csv.Data;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagement.Services.Csv;

/// <summary>
/// Interface for a service to read and write CSV content to and from disk.
/// </summary>
public interface ICsvService
{
    bool TryReadCsvFile([NotNullWhen(true)] out CsvFile? csvFile);
    bool TryWriteCsvFile(Func<List<string>> csvContent);
}