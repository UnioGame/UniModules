using System.Collections;
using Tools.AsyncOperations;

public class AsyncRequestOperation : IAsyncOperation {

    private static long _id;
    
    public AsyncRequestOperation() {
        _id++;
        Id = _id;
    }
    
    public long Id { get; protected set; }
    
    public bool IsDone { get; protected set; }

    public string Error { get; protected set; }

    public bool Active { get; protected set; }

    public IEnumerator Execute() {

        if (Active) {
            yield return WaitActive();
        }

        if (OnValidate()) {

            Active = true;
            OnInitialize();

            if(IsIterationActive())
                yield return MoveNext();

            IsDone = true;
            OnComplete();

        }
        IsDone = true;
        Active = false;
    }

    public virtual void Release()
    {
        OnReset();
    }

    #region private methods

    protected virtual IEnumerator MoveNext() {
        yield break;
    }
    
    protected virtual void OnInitialize(){}
    
    protected virtual void OnReset(){}

    protected virtual void OnComplete() { }

    protected virtual bool IsIterationActive() {
        return true;
    }

    protected virtual bool OnValidate() {
        return IsDone == false && string.IsNullOrEmpty(Error);
    }

    private IEnumerator WaitActive() {
        while (IsDone == false) {
            yield return null;
        }
    }

    #endregion
}

