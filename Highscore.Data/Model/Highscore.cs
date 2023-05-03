using System.ComponentModel.DataAnnotations;

namespace Highscore.Data.Model;

public class Highscore
{
    [Key]
    public int Id { get; set; }

    public string PlayerName { get; set; }

    public string GameName { get; set; }

    public string ScoreValue { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string Comment { get; set; }
}