using Highscore.Code;
using Highscore.Code.AccessLayer;
using Highscore.Wpf.Infrastructure;
using Highscore.Wpf.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Highscore.Wpf;

public class MainWindowViewModel : BaseViewModel
{
    private readonly HighscoreAccessLayer _highscoreAccessLayer = new();
    private readonly ValidationService _validationService = new();

    public ObservableCollection<Data.Model.Highscore> Highscores { get; set; } = new();

    private Data.Model.Highscore _highscore = new();
    public Data.Model.Highscore Highscore
    {
        get => _highscore;
        set
        {
            if (_highscore == value) return;
            _highscore = value;
            OnPropertyChanged();
        }
    }

    private Data.Model.Highscore? _selectedHighscore = null;
    public Data.Model.Highscore? SelectedHighscore
    {
        get => _selectedHighscore;
        set
        {
            if(_selectedHighscore == value) return;
            _selectedHighscore = value;
            OnPropertyChanged();
        }
    }

    public List<string> Games { get; set; } = new();

    public ICommand AddCommand => new RelayCommand()
    {
        CommandAction = () =>
        {
            string error = _validationService.IsHighscoreValid(Highscore);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
                return;
            }

            _highscoreAccessLayer.Add(Highscore);

            Highscore = new();
            RefreshHighscoreData();
        }
    };

    public ICommand RemoveCommand => new RelayCommand()
    {
        CanExecuteFunc = () => SelectedHighscore is not null,
        CommandAction = () =>
        {
            _highscoreAccessLayer.Remove(SelectedHighscore!.Id);

            SelectedHighscore = null;
            RefreshHighscoreData();
        }
    };

    public void Loaded()
    {
        _highscoreAccessLayer.Seed();
        RefreshHighscoreData();
    }

    private void RefreshHighscoreData()
    {
        Highscores = new(_highscoreAccessLayer.GetAll());
        OnPropertyChanged(nameof(Highscores));

        Games = _highscoreAccessLayer.GetGames();
        OnPropertyChanged(nameof(Games));
    }
}