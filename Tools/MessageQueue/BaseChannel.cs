using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.MessageQueue;
using Assets.Scripts.ProfilerTools;
using UniRx;
using UnityEngine;

public class BaseChannel : IChannel
{
    protected Subject<IMessage> _input;
    protected Subject<IMessage> _output;
    protected Subject<Type> _typeAttached;
    protected Subject<Type> _typeRemoved;

    protected List<IDisposable> _disposables;
    protected HashSet<Type> _registeredTypes;

    public BaseChannel() {

        _input = new Subject<IMessage>();
        _output = new Subject<IMessage>();
        _disposables = new List<IDisposable>();
        _registeredTypes = new HashSet<Type>();

    }

    public bool IsDisposed { get; protected set; }

    public IObservable<IMessage> Input => _input;

    public IObservable<IMessage> Output => _output;

    public IObservable<Type> OnAttach => _typeAttached;

    public IObservable<Type> OnUnsubscribe => _typeRemoved;

    public void Send(IMessage message) {

        ProcessMessage(message, _output, ValidateOutputMessage);

    }

    public IEnumerable<Type> GetRegisteredTypes() {
        return _registeredTypes;
    }

    public bool IsRegistered(Type type) {
        return _registeredTypes.Contains(type);
    }

    public IDisposable Attach<T>(Action<T> messageAction, Func<T, bool> filter = null)
        where T : class {

        var targetType = typeof(T);
        var disposable = _input.Where(x => Filter<T>(x.Context, filter)).
            Subscribe(x => messageAction(x.Context as T)).AddTo(_disposables);
        _registeredTypes.Add(targetType);

        return disposable;

    }

    public void SendToInput(IMessage message) {

        ProcessMessage(message, _input, ValidateInputMessage);

    }
    
    public void Dispose()
    {
        _input.OnCompleted();
        _output.OnCompleted();

        _input.Dispose();
        _output.Dispose();

        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }

        _disposables.Clear();

        IsDisposed = true;
    }

    #region private methods

    
    private bool Filter<T>(object context, Func<T, bool> filter = null) where T: class {

        var data = context as T;
        if (data == null) return false;
        var result = filter?.Invoke(data) ?? true;
        return result;

    }

    protected virtual bool ValidateOutputMessage(IMessage message) {

        return true;

    }

    protected virtual bool ValidateInputMessage(IMessage data) {

        return true;

    }


    private void ProcessMessage(IMessage messageObject,IObserver<IMessage> observable, Func<IMessage,bool> validator)
    {
        if (messageObject == null) return;

        var validateResult = validator(messageObject);
        if (validateResult == false) return;
        observable.OnNext(messageObject);

    }

    #endregion
}
