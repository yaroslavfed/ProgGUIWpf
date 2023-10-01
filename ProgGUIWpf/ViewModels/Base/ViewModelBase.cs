using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProgGUIWpf.ViewModels.Base;

internal abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
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

    
    // TODO: Возможно деструктор не нужен, но хабр говорит обратное))
    ~ViewModelBase()
    {
        Dispose(false);
    }
    
    public void Dispose()
    {
        Dispose(true);
    }

    private bool _Disposed;
    
    // Освобождение управляемых ресурсов
    protected virtual void Dispose(bool Disposing)
    {
        if(!Disposing || _Disposed)
            return;
        _Disposed = true;
    }
}