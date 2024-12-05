namespace HobbyManagement.Services.Csv.Data;

public class CsvCell
{
    public CsvCell(CsvColumn column, string value)
    {
        Column = column;
        Value = value;
    }

    public CsvColumn Column { get; set; }
    public string Value { get; set; } = "";
}
