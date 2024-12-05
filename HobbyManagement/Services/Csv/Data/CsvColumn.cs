namespace HobbyManagement.Services.Csv.Data;

public class CsvColumn
{
    public CsvColumn(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = "";
}
