﻿using Highscore.Code.AccessLayer;
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
    private readonly ValidationService _validationService = new();
    private readonly CsvService _csvService = new();

    private IHighscoreAccessLayer _highscoreAccessLayer;
    private string _csvPath = string.Empty;

    public MainWindowViewModel() => GetPathFromUser();

    public ObservableCollection<Data.Model.Highscore> Highscores { get; set; } = new();   // Initializes an empty ObservableCollection of type Data.Model.Highscore.

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
            if (_selectedHighscore == value) return;
            _selectedHighscore = value;
            OnPropertyChanged();
        }
    }

    public List<string> Games { get; set; } = new();   // Initializes an empty List of strings.

    public ICommand AddCommand => new RelayCommand()
    {
        CommandAction = () =>
        {
            string error = _validationService.IsHighscoreValid(Highscore); // Calls the IsHighscoreValid method of the _validationService instance.
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
            _highscoreAccessLayer.Remove(SelectedHighscore!.Id); // Calls the Remove method of the _highscoreAccessLayer instance.

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

            List<Data.Model.Highscore> highscores = _highscoreAccessLayer.GetAll(); _highscoreAccessLayer.GetAll(); // Calls the GetAll method of the _highscoreAccessLayer instance.
            string filename = $"csv_export_{DateTime.Now.ToString("yyyy mm dd hh mm ss")}_{highscores.Count}"; // Creates a filename string.
            string path = dialog.SelectedPath + "\\" + filename;

            _csvService.ExportCSV<Data.Model.Highscore>(highscores, path);
        }
    };

    public void Loaded() => RefreshHighscoreData();

    private void RefreshHighscoreData()
    {
        Highscores = new(_highscoreAccessLayer.GetAll()); // Retrieves all highscores from the _highscoreAccessLayer and assigns them to the Highscores list.
        OnPropertyChanged(nameof(Highscores));       // Notifies that the Highscores property has changed.

        Games = _highscoreAccessLayer.GetGames();
        OnPropertyChanged(nameof(Games));
    }

    private void GetPathFromUser()
    {
        MessageBox.Show("Select a folder for your highscores file");
        FolderBrowserDialog dialog = new();

        if (dialog.ShowDialog() == DialogResult.OK) // Displays the folder browser dialog and checks if the user clicked OK.
        {
            _csvPath = dialog.SelectedPath + "/highscores.csv";
            _highscoreAccessLayer = new HighscoreCsvAccessLayer(_csvPath);
        }
        else
        {
            MessageBox.Show("Folder path not valid");
            Environment.Exit(1);
        }
    }
}