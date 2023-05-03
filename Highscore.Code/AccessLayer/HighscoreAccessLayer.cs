using Highscore.Data.Model;

namespace Highscore.Code.AccessLayer;

public class HighscoreAccessLayer
{
    private readonly HighscoreDbContext _dbContext = HighscoreDbContext.GetContext();

    public List<Data.Model.Highscore> GetAll()
    {
        return _dbContext.Highscores.ToList();
    }

    public List<string> GetGames()
    {
        List<string> games = _dbContext.Highscores.Select(x => x.GameName).Distinct().ToList();
        return games;
    }

    public bool Remove(int id)
    {
        Data.Model.Highscore? highscore = _dbContext.Highscores.FirstOrDefault(x => x.Id == id);
        
        if(highscore is null)
            return false;

        _dbContext.Highscores.Remove(highscore);
        _dbContext.SaveChanges();

        return !_dbContext.Highscores.Contains(highscore);
    }

    public bool Add(Data.Model.Highscore highscore)
    {
        _dbContext.Highscores.Add(highscore);
        _dbContext.SaveChanges();

        return _dbContext.Highscores.Contains(highscore);
    }

    public void Seed() => _dbContext.Seed();
}