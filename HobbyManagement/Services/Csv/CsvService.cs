using HobbyManagement.Services.Csv.Data;
using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace HobbyManagement.Services.Csv;

/// <summary>
/// A service to read and write CSV content to and from disk.
/// </summary>
public class CsvService : ICsvService
{
    #region Constants

    /// <summary>
    /// The filter for the select file dialog.
    /// </summary>
    private const string CsvFileDialogFilter = "CSV files (*.csv)|*.csv";

    #endregion

    #region Methods

    /// <summary>
    /// Attempts to read the content from a csv file on disk. The user is presented with a open file dialog. 
    /// </summary>
    /// <param name="csvFile">The csv content in the form of a <see cref="CsvFile"/> if the operation was succesful.</param>
    /// <returns>True if the operation was successful. False if the user did not chose a file.</returns>
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

    /// <summary>
    /// Attempts to write csv content to a file on disk. 
    /// </summary>
    /// <param name="csvContent">The rows to write to the file.</param>
    /// <returns>True if the operation was successful.</returns>
    /// <exception cref="ArgumentException"></exception>
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
