using System;
using System.Windows;
using System.Windows.Input;
using ProgGUIWpf.Infrastructures.Commands;
using ProgGUIWpf.ViewModels.Base;

namespace ProgGUIWpf.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    #region Fields

    private string _title = "Главное окно";

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
            new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
    }

    #endregion

    #region Commands

    public ICommand CloseApplicationCommand { get; }

    #endregion

    #region Methods

        #region CloseApplicationCommandExecute

    private void OnCloseApplicationCommandExecuted(object parameter) => Application.Current.Shutdown();

    private bool CanCloseApplicationCommandExecute(object parameter) => true;
    
        #endregion

    #endregion
}