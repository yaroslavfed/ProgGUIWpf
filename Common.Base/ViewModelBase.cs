using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Base;

public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
{
    #region Fields

    public event PropertyChangedEventHandler? PropertyChanged;

    // TODO: Возможно деструктор тут не нужен, всё таки приложение не подразумевает наличие обработчика нескольких окон
    private bool _disposed;

    #endregion

    #region Methods

    protected virtual void InvokePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
            return false;
        field = value;
        InvokePropertyChanged(propertyName);
        return true;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;
        _disposed = true;

        // Освобождение управляемых ресурсов
    }

    #endregion

    #region Destructor

    ~ViewModelBase()
    {
        Dispose(false);
    }

    #endregion
}