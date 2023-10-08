using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Application.GUIWpf.Infrastructures.Commands;
using Application.GUIWpf.Infrastructures.Interfaces;
using Application.GUIWpf.Models;
using Application.GUIWpf.Services.Readers;
using Common.Base;

namespace Application.GUIWpf.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    #region Private fields

    private string _title = "Главное окно";

    private DataLocation _selectedFile = default!;

    #endregion

    #region Public fields

    public ObservableCollection<DataLocation> DataLocations { get; }

    public DataLocation SelectedFile
    {
        get => _selectedFile;
        set => Set(ref _selectedFile, value);
    }

    #endregion

    #region Commands

    #region SystemCommands

    public ICommand CloseApplicationCommand { get; }

    public ICommand ReduceApplicationCommand { get; }

    public ICommand WrapApplicationCommand { get; }

    #endregion

    #region FileSystemCommands

    public ICommand CreateNewFileCommand { get; }

    public ICommand DeleteNewFileCommand { get; }

    public ICommand UploadNewFileCommand { get; }

    public ICommand SaveToFileCommand { get; }

    #endregion

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
        #region Commands initialization

        CloseApplicationCommand = new Command(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

        WrapApplicationCommand = new Command(OnWrapApplicationCommandExecuted, CanWrapApplicationCommandExecute);

        ReduceApplicationCommand = new Command(OnReduceApplicationCommandExecuted, CanReduceApplicationCommandExecute);

        CreateNewFileCommand = new Command(OnCreateNewFileCommandExecuted, CanCreateNewFileCommandExecute);

        DeleteNewFileCommand = new Command(OnDeleteNewFileCommandExecuted, CanDeleteNewFileCommandExecute);

        UploadNewFileCommand = new Command(OnUploadNewFileCommandExecuted, CanUploadNewFileCommandExecute);

        SaveToFileCommand = new Command(OnSaveToFileCommandExecuted, CanSaveToFileCommandExecute);

        #endregion

        #region FileSystem initialization

        // TODO: убрать сидирование, заменить на получение его из файла
        var indexSeedPoint = 1;

        // var points = Enumerable.Range(1, new Random().Next(2, 10)).Select(i => new Location
        // {
        //     PointX = indexSeedPoint,
        //     PointY = Math.Pow(indexSeedPoint++, 2)
        // });

        var dataTemplate = Enumerable.Range(1, new Random().Next(2, 5)).Select(i => new DataLocation
        {
            Namespace = $"File {i}",
            LocationsList = new ObservableCollection<ICoordinatesCollection>(Enumerable
                .Range(1, new Random().Next(2, 10)).Select(j => new Location
                {
                    PointX = indexSeedPoint,
                    PointY = Math.Pow(indexSeedPoint++, 2)
                }))
        });

        #endregion

        DataLocations = new ObservableCollection<DataLocation>(dataTemplate);
    }

    #endregion

    #region Methods

    #region CloseApplicationCommandExecute

    private void OnCloseApplicationCommandExecuted(object parameter) => System.Windows.Application.Current.Shutdown();

    private bool CanCloseApplicationCommandExecute(object parameter) => true;

    #endregion

    #region ReduceApplicationCommandExecute

    private void OnReduceApplicationCommandExecuted(object parameter)
    {
        var winState = System.Windows.Application.Current.MainWindow!.WindowState;

        if (winState == WindowState.Normal)
            System.Windows.Application.Current.MainWindow!.WindowState = WindowState.Maximized;
        else
            System.Windows.Application.Current.MainWindow!.WindowState = WindowState.Normal;
    }

    private bool CanReduceApplicationCommandExecute(object parameter) => true;

    #endregion

    #region WrapApplicationCommandExecute

    private void OnWrapApplicationCommandExecuted(object parameter) =>
        System.Windows.Application.Current.MainWindow!.WindowState = WindowState.Minimized;

    private bool CanWrapApplicationCommandExecute(object parameter) => true;

    #endregion

    // TODO: переделать на работу с файловой системой

    #region CreateNewFileCommandExecute

    private void OnCreateNewFileCommandExecuted(object parameter)
    {
        var dataLocationIndex = DataLocations.Count + 1;

        DataLocations.Add(
            new DataLocation
            {
                Namespace = $"File {dataLocationIndex}",
                LocationsList = new ObservableCollection<ICoordinatesCollection>()
            });
    }

    private bool CanCreateNewFileCommandExecute(object parameter) => true;

    #endregion

    // TODO: переделать на работу с файловой системой

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

    // TODO: переделать на работу с файловой системой

    #region UploadNewFileCommandExecute

    private async void OnUploadNewFileCommandExecuted(object parameter)
    {
        var csvReader = new CsvReader("ViewModels\\testCsv.csv");
        await csvReader.Startup();
    }

    private bool CanUploadNewFileCommandExecute(object parameter) =>
        true;

    #endregion
    
    // TODO: переделать на работу с файловой системой

    #region SaveToFileCommandExecute

    private void OnSaveToFileCommandExecuted(object parameter)
    {
        if (parameter is not DataLocation dataLocation)
            return;
        
        MessageBox.Show($"Saving file {dataLocation.Namespace}...");
    }

    private bool CanSaveToFileCommandExecute(object parameter) =>
        parameter is DataLocation dataLocation && DataLocations.Contains(dataLocation);

    #endregion

    #endregion
}