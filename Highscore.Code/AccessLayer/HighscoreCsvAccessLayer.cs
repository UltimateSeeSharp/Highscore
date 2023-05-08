using CsvHelper;
using System.Globalization;

namespace Highscore.Code.AccessLayer;

public class HighscoreCsvAccessLayer : IHighscoreAccessLayer
{
    private readonly string _csvPath;

    public HighscoreCsvAccessLayer(string path)
    {
        _csvPath = path;
        EnsureStorageCreated();
    }

    public void EnsureStorageCreated()
    {
        if (File.Exists(_csvPath))              // Checks if the CSV file already exists                    
            return;

        using StreamWriter writer = new(this._csvPath);
        using CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture);

        csvWriter.WriteHeader<Data.Model.Highscore>(); // Writes the headers based on the Highscore model
        csvWriter.NextRecord();// moves to the next line
        csvWriter.Flush();// Writes the buffer to the file
    }

    public void Add(Data.Model.Highscore highscore)
    {
        highscore.Id = GetAll().Count + 1;  // Assigns a new ID based on the number of existing highscores

        using StreamWriter writer = new(this._csvPath, append: true);           // Creates a StreamWriter to write the CSV file
        using CsvWriter csvWriter = new(writer, CultureInfo.InvariantCulture);  // Creates a CsvWriter with the StreamWriter and the current culture

        csvWriter.WriteRecord<Data.Model.Highscore>(highscore);  // Writes the given highscore to the CSV file
        csvWriter.Flush(); 
    }

    public void AddRange(List<Data.Model.Highscore> highscores)
    {
        highscores.ForEach(x => Add(x));   // Adds a list of highscores by calling Add() for each highscore
    }

    public List<Data.Model.Highscore> GetAll()
    {
        using StreamReader reader = new(this._csvPath);    // Creates a StreamReader to read the CSV file
        using CsvReader csvReader = new(reader, CultureInfo.InvariantCulture);// Creates a CsvReader with the StreamReader and the current culture

        List<Data.Model.Highscore> highscores = csvReader.GetRecords<Data.Model.Highscore>().ToList();

        return highscores.OrderByDescending(x => Convert.ToInt32(x.ScoreValue)).ToList();// Returns the highscores in descending order of ScoreValue
    }

    public List<string> GetGames()
    {
        return GetAll().Select(x => x.GameName).Distinct().ToList(); // Returns a list of unique game names by retrieving the GameName property of all highscores
    }

    public void Remove(int id)
    {
        List<Data.Model.Highscore> highscores = GetAll();
        
        Data.Model.Highscore? highscore = highscores.FirstOrDefault(x => x.Id == id);  // Searches for the highscore with the specified ID
        if (highscore is null)
            return;

        highscores.Remove(highscore);
        
        File.Delete(this._csvPath);

        EnsureStorageCreated();
        AddRange(highscores);
    }
}