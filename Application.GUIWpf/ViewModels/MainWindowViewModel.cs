using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Application.GUIWpf.Infrastructures.Commands;
using Application.GUIWpf.Models;
using Common.Base;

namespace Application.GUIWpf.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    #region Private Fields

    private string _title = "Главное окно";

    private DataLocation _selectedFile = default!;

    #endregion

    #region Public Fields

    public ObservableCollection<DataLocation> DataLocations { get; }

    public DataLocation SelectedFile
    {
        get => _selectedFile;
        set => Set(ref _selectedFile, value);
    }

    #endregion

    #region Commands

    public ICommand CloseApplicationCommand { get; }

    public ICommand CreateNewFileCommand { get; }

    public ICommand DeleteNewFileCommand { get; }

    #endregion

    #region Properties

    /// <summary>Заголовок окна</summary> 
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    #endregion

    #region Constructors

    public MainWindowViewModel()
    {
        CloseApplicationCommand =
            new Command(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

        CreateNewFileCommand = new Command(OnCreateNewFileCommandExecuted, CanCreateNewFileCommandExecute);

        DeleteNewFileCommand = new Command(OnDeleteNewFileCommandExecuted, CanDeleteNewFileCommandExecute);

        // TODO: убрать сидирование, заменить на получение его из файла
        var indexSeedPoint = 1;
        var points = Enumerable.Range(1, 3).Select(i => new Location
        {
            PointX = indexSeedPoint,
            PointY = Math.Pow(indexSeedPoint++, 2)
        });

        var dataTemplate = Enumerable.Range(1, 2).Select(i => new DataLocation
        {
            Namespace = $"File {i}",
            LocationsList = new ObservableCollection<Location>(points)
        });

        DataLocations = new ObservableCollection<DataLocation>(dataTemplate);
    }

    #endregion

    #region Methods

    #region CloseApplicationCommandExecute

    private void OnCloseApplicationCommandExecuted(object parameter) => System.Windows.Application.Current.Shutdown();

    private bool CanCloseApplicationCommandExecute(object parameter) => true;

    #endregion

    #region CreateNewFileCommandExecute

    private void OnCreateNewFileCommandExecuted(object parameter)
    {
        var dataLocationIndex = DataLocations.Count + 1;
        DataLocations.Add(
            new DataLocation
            {
                Namespace = $"File {dataLocationIndex}",
                LocationsList = new ObservableCollection<Location>()
            });
    }

    private bool CanCreateNewFileCommandExecute(object parameter) => true;

    #endregion

    #region DeleteNewFileCommandExecute

    private void OnDeleteNewFileCommandExecuted(object parameter)
    {
        if (parameter is not DataLocation dataLocation)
            return;
        var dataLocationIndex = DataLocations.IndexOf(dataLocation);
        
        DataLocations.Remove(dataLocation);
        
        if (dataLocationIndex < DataLocations.Count)
            SelectedFile = DataLocations[dataLocationIndex];
    }

    private bool CanDeleteNewFileCommandExecute(object parameter) =>
        parameter is DataLocation dataLocation && DataLocations.Contains(dataLocation);

    #endregion

    #endregion
}