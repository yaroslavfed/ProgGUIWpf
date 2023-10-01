using System;
using ProgGUIWpf.ViewModels.Base;

namespace ProgGUIWpf.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private string _title = "Главное окно";
    
    /// <summary>Заголовок окна</summary> 
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }
}