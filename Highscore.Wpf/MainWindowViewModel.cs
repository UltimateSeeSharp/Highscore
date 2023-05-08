using Highscore.Code.AccessLayer;
using Highscore.Wpf.Infrastructure;
using Highscore.Wpf.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace Highscore.Wpf;

public class MainWindowViewModel : BaseViewModel
{
    private readonly IHighscoreAccessLayer _highscoreAccessLayer = new HighscoreCsvAccessLayer();
    private readonly ValidationService _validationService = new();
    private readonly CsvService _csvService = new();

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
                System.Windows.MessageBox.Show(error);
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

    public ICommand ImportCSV => new RelayCommand()
    {
        CommandAction = () =>
        {
            OpenFileDialog dialog = new();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            List<Data.Model.Highscore> highscores = _csvService.ImportCsv<Data.Model.Highscore>(dialog.FileName);
            _highscoreAccessLayer.AddRange(highscores);
        }
    };

    public ICommand ExportCSV => new RelayCommand()
    {
        CommandAction = () =>
        {
            FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            List<Data.Model.Highscore> highscores = _highscoreAccessLayer.GetAll();
            string filename = $"csv_export_{DateTime.Now.ToString("yyyy mm dd hh mm ss")}_{highscores.Count}";
            string path = dialog.SelectedPath + "\\" + filename;

            _csvService.ExportCSV<Data.Model.Highscore>(highscores, path);        
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