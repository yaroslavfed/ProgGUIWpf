using System;
using ProgGUIWpf.ViewModels.Base;

namespace ProgGUIWpf.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    #region Fields

    private string _title = "Главное окно";

    #endregion
    
    #region Methods
    
    /// <summary>Заголовок окна</summary> 
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }
    
    #endregion
}