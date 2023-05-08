using CsvHelper;
using System.Globalization;

namespace Highscore.Code.AccessLayer;

public class HighscoreCsvAccessLayer : IHighscoreAccessLayer
{
    private readonly string _csvPath = "C:\\Users\\Schneider David\\Desktop\\HighscoreProject\\highscores.csv";

    public HighscoreCsvAccessLayer() => EnsureStorageCreated();

    public void EnsureStorageCreated()
    {
        if (File.Exists(_csvPath))
            return;

        using StreamWriter writer = new(this._csvPath);
        using CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture);

        csvWriter.WriteHeader<Data.Model.Highscore>();
        csvWriter.NextRecord();
        csvWriter.Flush();
    }

    public void Add(Data.Model.Highscore highscore)
    {
        highscore.Id = GetAll().Count + 1;

        using StreamWriter writer = new(this._csvPath, append: true);
        using CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture);

        csvWriter.WriteRecord<Data.Model.Highscore>(highscore);
        csvWriter.NextRecord();
        csvWriter.Flush();
    }

    public void AddRange(List<Data.Model.Highscore> highscores)
    {
        highscores.ForEach(x => Add(x));
    }

    public List<Data.Model.Highscore> GetAll()
    {
        using StreamReader reader = new(this._csvPath);
        using CsvReader csvReader = new(reader, CultureInfo.InvariantCulture);

        List<Data.Model.Highscore> highscores = csvReader.GetRecords<Data.Model.Highscore>().ToList();

        return highscores.OrderByDescending(x => Convert.ToInt32(x.ScoreValue)).ToList();
    }

    public List<string> GetGames()
    {
        return GetAll().Select(x => x.GameName).Distinct().ToList();
    }

    public void Remove(int id)
    {
        List<Data.Model.Highscore> highscores = GetAll();
        
        Data.Model.Highscore? highscore = highscores.FirstOrDefault(x => x.Id == id);
        if (highscore is null)
            return;

        highscores.Remove(highscore);
        
        File.Delete(this._csvPath);

        EnsureStorageCreated();
        AddRange(highscores);
    }
}