using System;
using Common.Base.Abstractions;

namespace Application.GUIWpf.Infrastructures.Commands;

internal class Command : CommandBase
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool>? _canExecute;

    public Command(Action<object> execute, Func<object, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter!) ?? true;

    public override void Execute(object? parameter) => _execute?.Invoke(parameter!);
}