using HobbyManagement.Services.Csv.Error;
using System.Text.RegularExpressions;

namespace HobbyManagement.Services.Csv.Data;

public class CsvFile
{
    private CsvFile(List<CsvColumn> csvColumns, List<CsvRow> csvRows)
    {
        CsvColumns = csvColumns;
        CsvRows = csvRows;
    }

    public List<CsvColumn> CsvColumns { get; } = new();
    public List<CsvRow> CsvRows { get; } = new();

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
}
