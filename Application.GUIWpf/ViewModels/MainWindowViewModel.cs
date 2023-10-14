using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application.GUIWpf.Infrastructures.Commands;
using Application.GUIWpf.Models;
using Application.GUIWpf.Services.Readers;
using Common.Base.Abstractions;
using Common.Base.Converters;
using Common.Base.Interfaces;

namespace Application.GUIWpf.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region Private fields

    private string _title = "Главное окно";

    private IReaderSupport _selectedFile = default!;

    #endregion

    #region Public fields

    public ObservableCollection<IReaderSupport> DataLocations { get; } = new();

    public IReaderSupport SelectedFile
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

        System.Windows.Application.Current.MainWindow!.WindowState =
            winState == WindowState.Normal 
                ? WindowState.Maximized 
                : WindowState.Normal;
    }

    private bool CanReduceApplicationCommandExecute(object parameter) => true;

    #endregion

    #region WrapApplicationCommandExecute

    private void OnWrapApplicationCommandExecuted(object parameter) =>
        System.Windows.Application.Current.MainWindow!.WindowState = WindowState.Minimized;

    private bool CanWrapApplicationCommandExecute(object parameter) => true;

    #endregion

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

    #region DeleteNewFileCommandExecute

    private void OnDeleteNewFileCommandExecuted(object parameter)
    {
        if (parameter is not IReaderSupport dataLocation)
            return;
        var dataLocationIndex = DataLocations.IndexOf(dataLocation);

        DataLocations.Remove(dataLocation);

        if (dataLocationIndex < DataLocations.Count)
            SelectedFile = DataLocations[dataLocationIndex];
    }

    private bool CanDeleteNewFileCommandExecute(object parameter) =>
        parameter is DataLocation dataLocation && DataLocations.Contains(dataLocation);

    #endregion

    #region UploadNewFileCommandExecute

    private async void OnUploadNewFileCommandExecuted(object parameter)
    {
        ReaderBase csvReaderBase = new CsvReader()
        {
            FileExtension = ".csv"
        };

        var result = await csvReaderBase.StartupAsync();

        if (result != null)
            DataLocations.Add(result);
    }

    private bool CanUploadNewFileCommandExecute(object parameter) =>
        true;

    #endregion

    #region SaveToFileCommandExecute

    private void OnSaveToFileCommandExecuted(object parameter)
    {
        if (parameter is not IReaderSupport dataLocation)
            return;

        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = dataLocation.Namespace,
            DefaultExt = ".csv",
            Filter = "Text documents (.csv)|*.csv"
        };

        var result = dialog.ShowDialog();

        if (result != true) return;

        var filename = dialog.FileName;

        var sb = new StringBuilder();
        foreach (var data in dataLocation.LocationsList)
            sb.AppendLine(data.ToCsvFormat());

        File.WriteAllText(filename, sb.ToString());
        MessageBox.Show($"Файл {filename.ToShortFileName()} сохранен");
    }

    private bool CanSaveToFileCommandExecute(object parameter) =>
        parameter is IReaderSupport dataLocation && DataLocations.Contains(dataLocation);

    #endregion

    #endregion
}