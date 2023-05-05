using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Highscore.Code.AccessLayer;

public class HighscoreCsvAccessLayer : IHighscoreAccessLayer
{
    private readonly string _csvPath = "C:\\Users\\Schneider David\\Desktop\\HighscoreProject\\highscores.csv";

    public void Add(Data.Model.Highscore highscore)
    {
        highscore.Id = GetNextId();

        using StreamWriter writer = new(_csvPath);
        using CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture);

        csvWriter.WriteRecord<Data.Model.Highscore>(highscore);
        writer.Flush();
    }

    public void AddRange(List<Data.Model.Highscore> highscores)
    {
        foreach (Data.Model.Highscore highscore in highscores)
        {
            Add(highscore);
        }
    }

    public List<Data.Model.Highscore> GetAll()
    {
        using StreamReader reader = new(_csvPath);
        using CsvReader csvReader = new(reader, CultureInfo.InvariantCulture);

        List<Data.Model.Highscore> records = csvReader.GetRecords<Data.Model.Highscore>().ToList();
        reader.Close();

        return records;
    }

    public List<string> GetGames()
    {
        List<Data.Model.Highscore> highscores = GetAll();
        List<string> games = highscores.Select(x => x.GameName).Distinct().ToList();
        return games;
    }

    public void Remove(int id)
    {
        List<Data.Model.Highscore> highscores = GetAll();
        Data.Model.Highscore? highscore = highscores.FirstOrDefault(x => x.Id == id);

        if (highscore is null)
            return;

        highscores.Remove(highscore);

        File.Delete(_csvPath);
        File.Create(_csvPath);

        using StreamWriter writer = new(_csvPath);
        using CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords<Data.Model.Highscore>(highscores);
        writer.Close();
    }

    public void Seed()
    {
        AddRange(new()
        {
            new()
            {
                PlayerName = "David",
                GameName = "Tetris",
                Comment = "Best score #1",
                Date = DateTime.Now,
                Id = 1,
                ScoreValue = "364"
            },
            new()
            {
                PlayerName = "Dave",
                GameName = "Tetris",
                Comment = "Best score #2",
                Date = DateTime.Now,
                Id = 2,
                ScoreValue = "365"
            }
        });
    }

    private int GetNextId() => GetAll().Count > 0 ? GetAll().Select(x => x.Id).Max() + 1 : 1;
}
