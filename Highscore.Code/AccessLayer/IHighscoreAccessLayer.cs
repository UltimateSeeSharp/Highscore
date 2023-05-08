namespace Highscore.Code.AccessLayer
{
    public interface IHighscoreAccessLayer
    {
        void Add(Data.Model.Highscore highscore);// Adds a single highscore entry to the storage.
        
        void AddRange(List<Data.Model.Highscore> highscores);// Adds a range of highscore entries to the storage.
        
        List<Data.Model.Highscore> GetAll();// Retrieves all highscore entries from the storage and returns them as a list.
        
        List<string> GetGames(); // Retrieves the names of all games for which highscores are stored and returns them as a list of strings.
       
        void Remove(int id);  // Removes a highscore entry with the specified ID from the storage.
      
        void EnsureStorageCreated(); // Ensures that the storage for highscores is created, if it doesn't exist already.
       
    }
}