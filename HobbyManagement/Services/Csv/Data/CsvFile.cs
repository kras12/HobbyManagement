using HobbyManagement.Services.Csv.Error;
using System.Text.RegularExpressions;

namespace HobbyManagement.Services.Csv.Data;

/// <summary>
/// Represents a Csv file.
/// </summary>
public class CsvFile
{
    #region Constructors
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="csvColumns">A collection of columns for the file.</param>
    /// <param name="csvRows">A collection of rows for the file.</param>
    private CsvFile(List<CsvColumn> csvColumns, List<CsvRow> csvRows)
    {
        CsvColumns = csvColumns;
        CsvRows = csvRows;
    }

    #endregion

    /// <summary>
    /// The columns in the file.
    /// </summary>
    public List<CsvColumn> CsvColumns { get; } = new();

    /// <summary>
    /// The rows in the file. 
    /// </summary>
    public List<CsvRow> CsvRows { get; } = new();

    #region Methods

    /// <summary>
    /// Creates a Csv file.
    /// </summary>
    /// <param name="csvContentRows">A collection of content rows design the file around.</param>
    /// <returns><see cref="CsvFile"/></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidCsvFormatException"></exception>
    public static CsvFile CreateFile(List<string> csvContentRows)
    {
        if (csvContentRows.Count == 0)
        {
            throw new ArgumentException($"The {nameof(csvContentRows)} collection can't be empty");
        }

        List<CsvColumn> csvColumns = ExtractRowParts(csvContentRows.Take(1).First())
                .Select(x => new CsvColumn(x))
                .ToList();

        List<CsvRow> csvRows = new();

        foreach (var csvContentRow in csvContentRows.Skip(1))
        {
            var cells = ExtractRowParts(csvContentRow);

            if (csvColumns.Count != cells.Count)
            {
                throw new InvalidCsvFormatException("Number of row cells doesn't match the number of columns.");
            }

            List<CsvCell> csvRowCells = csvColumns
                .Select((column, index) => new CsvCell(column, cells[index]))
                .ToList();

            csvRows.Add(new CsvRow(csvRowCells));
        }

        return new CsvFile(csvColumns, csvRows);
    }

    /// <summary>
    /// Extracts the cell values for a csv row. 
    /// </summary>
    /// <param name="row"></param>
    /// <returns>A collection of strings.</returns>
    private static List<string> ExtractRowParts(string row)
    {
        var pattern = @"(?<=^|,)\s*""?(?<field>(?:[^""]|"""")*)""?\s*(?=,|$)";

        var matches = Regex.Matches(row, pattern);

        var result = matches.Cast<Match>()
            .Select(m => m.Groups["field"]
            .Value.Replace("\"\"", "\""))
            .ToList();

        return result;
    }

    #endregion
}
