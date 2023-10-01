using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProgGUIWpf.ViewModels.Base;

internal abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void InvokePropertyChanged([CallerMemberName] string PropertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
    {
        if(Equals(field, value))
            return false;
        field = value;
        InvokePropertyChanged(PropertyName);
        return true;
    }
}