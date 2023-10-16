using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    #region Fields

    /// <summary>
    /// Заголовок окна
    /// </summary>
    private string _title = "Главное окно";

    /// <summary>
    /// Выбранный в данный момент файл
    /// </summary>
    private IReaderSupport _selectedFile = default!;

    /// <summary>
    /// Коллекция файлов с точками
    /// </summary>
    public IList<IReaderSupport> DataLocations { get; } = new ObservableCollection<IReaderSupport>();

    #endregion

    #region Properties

    /// <summary>
    /// Заголовок окна Property
    /// </summary> 
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    /// <summary>
    /// Выбранный в данный момент файл Property
    /// </summary>
    public IReaderSupport SelectedFile
    {
        get => _selectedFile;
        set => Set(ref _selectedFile, value);
    }

    #endregion

    #region Commands

    #region SystemCommands

    /// <summary>
    /// Команда для закрытия окна приложения
    /// </summary>
    public ICommand CloseApplicationCommand { get; }

    /// <summary>
    /// Команда для уменьшения размеров окна приложения
    /// </summary>
    public ICommand ReduceApplicationCommand { get; }

    /// <summary>
    /// Команда для сворачивания окна приложения
    /// </summary>
    public ICommand WrapApplicationCommand { get; }

    #endregion

    #region FileSystemCommands

    /// <summary>
    /// Команда для создания нового пустого файла
    /// </summary>
    public ICommand CreateNewFileCommand { get; }

    /// <summary>
    /// Команда для удаления выбранного файла
    /// </summary>
    public ICommand DeleteNewFileCommand { get; }

    /// <summary>
    /// Команда загрузки пользовательского файла в программу
    /// </summary>
    public ICommand UploadNewFileCommand { get; }

    /// <summary>
    /// Команда сохранения выбранного файла пользователю
    /// </summary>
    public ICommand SaveToFileCommand { get; }

    #endregion

    #region PointsSystemCommands

    /// <summary>
    /// Команда добавления новой точки в текущий файл
    /// </summary>
    public ICommand AddNewPointCommand { get; }

    /// <summary>
    /// Команда удаления последней точки из файла
    /// </summary>
    public ICommand DeletePointCommand { get; }

    #endregion

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

        AddNewPointCommand = new Command(OnAddNewPointCommandExecuted, CanAddNewPointCommandExecute);

        DeletePointCommand = new Command(OnDeletePointCommandExecuted, CanDeletePointCommandExecute);

        #endregion
    }

    #endregion

    #region Methods

    #region CloseApplicationCommand

    private void OnCloseApplicationCommandExecuted(object parameter) => System.Windows.Application.Current.Shutdown();

    private bool CanCloseApplicationCommandExecute(object parameter) => true;

    #endregion

    #region ReduceApplicationCommand

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

    #region WrapApplicationCommand

    private void OnWrapApplicationCommandExecuted(object parameter) =>
        System.Windows.Application.Current.MainWindow!.WindowState = WindowState.Minimized;

    private bool CanWrapApplicationCommandExecute(object parameter) => true;

    #endregion

    #region CreateNewFileCommand

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

    #region DeleteNewFileCommand

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

    #region UploadNewFileCommand

    private async void OnUploadNewFileCommandExecuted(object parameter)
    {
        ReaderBase csvReader = new CsvReader()
        {
            FileExtension = ".csv"
        };

        var result = await csvReader.StartupAsync();

        if (result != null)
            DataLocations.Add(
                new DataLocation
                {
                    Namespace = result.Namespace,
                    LocationsList = new ObservableCollection<ICoordinatesCollection>(result.LocationsList)
                });
    }

    private bool CanUploadNewFileCommandExecute(object parameter) =>
        true;

    #endregion

    #region SaveToFileCommand

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

    #region AddNewPointCommand

    private void OnAddNewPointCommandExecuted(object parameter)
    {
        DataLocations.FirstOrDefault(i => i == SelectedFile)?.LocationsList.Add(new Location());
    }

    private bool CanAddNewPointCommandExecute(object parameter)
    {
        return parameter is IReaderSupport dataLocation && DataLocations.Contains(dataLocation);
    }

    #endregion

    #region DeletePointCommand

    private void OnDeletePointCommandExecuted(object parameter)
    {
        var item = SelectedFile.LocationsList.LastOrDefault();
        if (item != null) SelectedFile.LocationsList.Remove(item);
    }

    private bool CanDeletePointCommandExecute(object parameter)
    {
        return parameter is IReaderSupport dataLocation && DataLocations.Contains(dataLocation) && dataLocation.LocationsList.Count > 0;
    }

    #endregion

    #endregion
}