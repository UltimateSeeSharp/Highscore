namespace Highscore.Code.AccessLayer
{
    public interface IHighscoreAccessLayer
    {
        void Add(Data.Model.Highscore highscore);
        void AddRange(List<Data.Model.Highscore> highscores);
        List<Data.Model.Highscore> GetAll();
        List<string> GetGames();
        void Remove(int id);
        void EnsureStorageCreated();
        void Seed();
    }
}