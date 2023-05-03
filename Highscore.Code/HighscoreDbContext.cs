using Microsoft.EntityFrameworkCore;

namespace Highscore.Code;

public class HighscoreDbContext : DbContext
{
    public HighscoreDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Data.Model.Highscore> Highscores { get; set; }

    private static HighscoreDbContext? _context = null;
    public static HighscoreDbContext GetContext()
    {
        if(_context != null)
            return _context;

        var builder = new DbContextOptionsBuilder<HighscoreDbContext>();
        builder.UseInMemoryDatabase("Highscores");

        return new HighscoreDbContext(builder.Options);
    }

    public void Seed()
    {
        Highscores.Add(new()
        {
            PlayerName = "David",
            GameName = "Tetris",
            Comment = "Best score",
            ScoreValue = "5253",
            Date = DateTime.Now
        });
        SaveChanges();
    }
}