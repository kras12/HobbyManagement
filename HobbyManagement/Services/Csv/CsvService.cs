using HobbyManagement.Services.Csv.Data;
using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace HobbyManagement.Services.Csv;

public class CsvService : ICsvService
{
    #region Constants

    private const string CsvFileDialogFilter = "CSV files (*.csv)|*.csv";

    #endregion

    #region Methods

    public bool TryReadCsvFile([NotNullWhen(true)] out CsvFile? csvFile)
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = CsvFileDialogFilter;
        openFileDialog.CheckFileExists = true;

        if (openFileDialog.ShowDialog() == true)
        {
            List<string> rows = File.ReadAllLines(openFileDialog.FileName).ToList();
            csvFile = CsvFile.CreateFile(rows);
            return true;
        }

        csvFile = null;
        return false;
    }

    public bool TryWriteCsvFile(Func<List<string>> csvContent)
    {
        SaveFileDialog saveFileDialog = new();
        saveFileDialog.Filter = CsvFileDialogFilter;
        saveFileDialog.Title = "Export File";
        saveFileDialog.OverwritePrompt = true;

        if (saveFileDialog.ShowDialog() == true)
        {
            List<string> fileContent = csvContent();

            if (fileContent.Count == 0)
            {
                throw new ArgumentException("The content collection can't be empty", nameof(csvContent));
            }

            try
            {
                File.WriteAllLines(saveFileDialog.FileName, fileContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to export file: {ex.Message}");
                throw;
            }

            return true;
        }

        return false;
    }

    #endregion
}
