namespace HobbyManagement.Services.Csv.Data;

/// <summary>
/// Represents a cell in a CsV file.
/// </summary>
public class CsvCell
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="column">The column the cell is associated with.</param>
    /// <param name="value">The value of the cell.</param>
    public CsvCell(CsvColumn column, string value)
    {
        Column = column;
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The column the cell is associated with.
    /// </summary>
    public CsvColumn Column { get; set; }

    /// <summary>
    /// The value of the cell.
    /// </summary>
    public string Value { get; set; } = "";

    #endregion
}
