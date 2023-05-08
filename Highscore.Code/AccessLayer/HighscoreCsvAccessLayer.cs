using CsvHelper;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Highscore.Code.AccessLayer;

public class HighscoreCsvAccessLayer : IHighscoreAccessLayer
{
    private readonly string _csvPath = "C:\\Users\\Schneider David\\Desktop\\HighscoreProject\\highscores.csv";
    private CsvWriter _csvWriter;

    public HighscoreCsvAccessLayer()
    {
        StreamWriter writer = new(this._csvPath);
        _csvWriter = new(writer, CultureInfo.InvariantCulture);

        WriteStorageFile();
    }

    private void WriteStorageFile()
    {
        if (!File.Exists(this._csvPath))
            File.Create(this._csvPath).Close();

        _csvWriter.WriteHeader<Data.Model.Highscore>();
        _csvWriter.NextRecord();
        _csvWriter.Flush();
    }

    public void Add(Data.Model.Highscore highscore)
    {
        _csvWriter
    }

    public void AddRange(List<Data.Model.Highscore> highscores)
    {
        throw new NotImplementedException();
    }

    public void EnsureStorageCreated()
    {

    }

    public List<Data.Model.Highscore> GetAll()
    {
        throw new NotImplementedException();
    }

    public List<string> GetGames()
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public void Seed()
    {
        var highscore = new Data.Model.Highscore
        {
            Comment = "test",
            Date = DateTime.Now,
            GameName = "re",
            PlayerName = "re",
            Id = 1,
            ScoreValue = "533"
        };

        _csvWriter.WriteRecord<Data.Model.Highscore>(highscore);
        _csvWriter.Flush();
    }
}