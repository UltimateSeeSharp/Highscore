using CsvHelper;
using Highscore.Data.Model;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System;

namespace Highscore.Wpf.Service;

internal class CsvService
{
    internal void ExportCSV<T>(List<Data.Model.Highscore> highscores, string path)
    {
        var sb = new StringBuilder();
        var properties = typeof(T).GetProperties();

        var header = string.Join(",", properties.Select(p => p.Name));
        sb.AppendLine(header);

        foreach (var highscore in highscores)
        {
            var row = string.Join(",", properties.Select(p => p.GetValue(highscore)?.ToString()));
            sb.AppendLine(row);
        }

        File.WriteAllText(path + ".csv", sb.ToString());
    }

    internal List<T> ImportCsv<T>(string path)
    {
        var list = new List<T>();
        var lines = File.ReadAllLines(path);

        if (lines.Length < 2)
        {
            return list;
        }

        var properties = typeof(T).GetProperties();

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var values = line.Split(',');
            var item = Activator.CreateInstance<T>();

            for (int j = 0; j < properties.Length; j++)
            {
                var property = properties[j];
                var value = values[j].Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(item, convertedValue);
                }
            }

            list.Add(item);
        }

        return list;
    }
}
