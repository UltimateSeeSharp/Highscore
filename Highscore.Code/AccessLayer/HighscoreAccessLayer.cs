namespace Highscore.Code.AccessLayer;

public class HighscoreAccessLayer : IHighscoreAccessLayer
{
    private HighscoreDbContext _dbContext = HighscoreDbContext.GetContext();

    public void Add(Data.Model.Highscore highscore)
    {
        _dbContext.Highscores.Add(highscore);
        _dbContext.SaveChanges();
    }

    public void AddRange(List<Data.Model.Highscore> highscores)
    {
        foreach (var highscore in highscores)
            _dbContext.Highscores.Add(highscore);

        _dbContext.SaveChanges();
    }

    public List<Data.Model.Highscore> GetAll()
    {
        List<Data.Model.Highscore> highscores = _dbContext.Highscores.ToList();
        return highscores.OrderByDescending(x => Convert.ToInt32(x.ScoreValue)).ToList();
    }

    public List<string> GetGames()
    {
        List<string> games = _dbContext.Highscores.Select(x => x.GameName).Distinct().ToList();
        return games;
    }

    public void Remove(int id)
    {
        Data.Model.Highscore? highscore = _dbContext.Highscores.FirstOrDefault(x => x.Id == id);

        if (highscore is null)
            return;

        _dbContext.Highscores.Remove(highscore);
        _dbContext.SaveChanges();
    }

    public void Seed() => _dbContext.Seed();
}