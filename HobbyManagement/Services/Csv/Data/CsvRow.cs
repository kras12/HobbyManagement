using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace HobbyManagement.Services.Csv.Data;

/// <summary>
/// Represents a csv row.
/// </summary>
public class CsvRow
{
    #region Fields
    
    /// <summary>
    /// A lookup table for the cells in the row. 
    /// </summary>
    private ReadOnlyDictionary<string, CsvCell> _cells;

    #endregion

    #region Constructors
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="cells">The cells to populate the row with.</param>
    public CsvRow(List<CsvCell> cells)
    {
        _cells = new(cells.ToDictionary(x => x.Column.Name, x => x));
    }

    #endregion

    #region Methods    

    /// <summary>
    /// Attempts to fetch the cell for the specified column.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="cell">Contains the cell if the operation was successful.</param>
    /// <returns>Returns true if the operation was successful.</returns>
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

    #endregion
}
