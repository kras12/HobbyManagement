using HobbyManagement.Services.Csv.Data;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagement.Services.Csv;
public interface ICsvService
{
    bool TryReadCsvFile([NotNullWhen(true)] out CsvFile? csvFile);
    bool TryWriteCsvFile(Func<List<string>> csvContent);
}