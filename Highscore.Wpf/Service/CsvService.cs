using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

namespace Highscore.Wpf.Service;

internal class CsvService
{
    internal void ExportCSV<T>(List<Data.Model.Highscore> highscores, string path)
    {
        var sb = new StringBuilder(); // Creates a new instance of StringBuilder to build the CSV content.
        var properties = typeof(T).GetProperties(); // Retrieves the properties of the specified type.

        var header = string.Join(",", properties.Select(p => p.Name)); // Builds the CSV header by joining the property names.
        sb.AppendLine(header); // Appends the header to the StringBuilder.

        foreach (var highscore in highscores)
        {
            var row = string.Join(",", properties.Select(p => p.GetValue(highscore)?.ToString())); // Builds a row of CSV values by retrieving the property values of the highscore object.
            sb.AppendLine(row); // Appends the row to the StringBuilder.
        }

        File.WriteAllText(path + ".csv", sb.ToString()); // Writes the CSV content to a file with the specified path.
    }

    internal List<T> ImportCsv<T>(string path)
    {
        var list = new List<T>(); 
        var lines = File.ReadAllLines(path); // Reads all lines from the CSV file.

        if (lines.Length < 2)
        {
            return list; 
        }

        var properties = typeof(T).GetProperties(); // Retrieves the properties of the specified type.

        for (int i = 1; i < lines.Length; i++) // Starts iterating from the second line (index 1) since the first line is the header.
        {
            var line = lines[i];
            var values = line.Split(','); 
            var item = Activator.CreateInstance<T>(); 

            for (int j = 0; j < properties.Length; j++) // Iterates over the properties.
            {
                var property = properties[j]; 
                var value = values[j].Trim(); // Gets the corresponding value from the array and trims whitespace.

                if (!string.IsNullOrEmpty(value))
                {
                    var convertedValue = Convert.ChangeType(value, property.PropertyType); // Converts the value to the property's type.
                    property.SetValue(item, convertedValue); // Sets the converted value to the property of the item object.
                }
            }

            list.Add(item); 
        }

        return list;
    }
}
