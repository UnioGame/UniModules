namespace UniGame.Utils
{
    using System;
    using System.Threading;
    using Abstract;

    public class TimeoutTaskExecutor<T> : ITaskExecutor<T>
    {
        private readonly ITaskExecutor<T> _taskExecutor;
        private readonly float _timeoutThresholdInSeconds;
        public CancellationTokenSource CancellationTokenSource => _taskExecutor.CancellationTokenSource;

        public TimeoutTaskExecutor(ITaskExecutor<T> taskExecutor, float timeoutThresholdInSeconds)
        {
            _taskExecutor = taskExecutor;
            
            _timeoutThresholdInSeconds = timeoutThresholdInSeconds;
        }

        public IObservable<TaskExecutionResult<T>> TryExecute()
        {
            _taskExecutor.CancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(_timeoutThresholdInSeconds));
            
            return _taskExecutor.TryExecute();
        }

        public void RaiseSuccessfulLoad(T result)
        {
            _taskExecutor.RaiseSuccessfulLoad(result);
        }

        public void RaiseFailedLoad()
        {
            _taskExecutor.RaiseFailedLoad();
        }

        public void Dispose()
        {
            _taskExecutor.Dispose();
        }
    }
}