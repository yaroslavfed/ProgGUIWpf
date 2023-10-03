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

    #endregion

    #region Public Fields

    public ObservableCollection<DataLocation> DataLocations { get; }

    #endregion
    
    #region Commands

    public ICommand CloseApplicationCommand { get; }

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

        // TODO: убрать сидирование, заменить на получение его из файла
        var points = Enumerable.Range(1, 3).Select(i => new Location
        {
            PointX = i,
            PointY = Math.Pow(i, 2)
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

    #endregion
}