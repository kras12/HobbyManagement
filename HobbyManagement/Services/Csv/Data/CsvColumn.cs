namespace HobbyManagement.Services.Csv.Data;

/// <summary>
/// Represents a column in a Csv file.
/// </summary>
public class CsvColumn
{
    #region Constructors

    /// <summary>
    /// Constructors.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    public CsvColumn(string name)
    {
        Name = name;
    }

    #endregion

    #region Properties
    
    /// <summary>
    /// The name of the column.
    /// </summary>
    public string Name { get; set; } = "";

    #endregion
}
