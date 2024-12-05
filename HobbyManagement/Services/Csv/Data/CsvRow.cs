using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagement.Services.Csv.Data;

public class CsvRow
{
    private ReadOnlyDictionary<string, CsvCell> _cells;

    public CsvRow(List<CsvCell> cells)
    {
        _cells = new(cells.ToDictionary(x => x.Column.Name, x => x));
    }

    public bool TryGetCell(string columnName, [NotNullWhen(true)] out CsvCell? cell)
    {
        if (_cells.ContainsKey(columnName))
        {
            cell = _cells[columnName];
            return true;
        }

        cell = null;
        return false;
    }
}
