using UniModules.UniCore.Runtime.AsyncOperations;

namespace UniGame.Utils
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ProfilerTools;
    using UniRx;

    public class TaskExecutor<T> : ITaskExecutor<T>
    {
        private readonly UniTask<T> _resultTaskHandle;
        private readonly IReactiveProperty<TaskExecutionResult<T>> _resultProperty;
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        private bool _executing;
        private bool _isFailed;

        public CancellationTokenSource CancellationTokenSource => _cancellationTokenSource;

        public TaskExecutor(UniTask<T> resultTaskHandle)
        {
            _resultTaskHandle = resultTaskHandle;
            
            _cancellationTokenSource = new CancellationTokenSource();
            _resultProperty = new ReactiveProperty<TaskExecutionResult<T>>();
        }

        public IObservable<TaskExecutionResult<T>> TryExecute()
        {
            if(!_executing) {
                _executing = true;

                if(!_isFailed) {
                    TryExecuteTask(_resultTaskHandle).Forget();
                }
            }

            return _resultProperty.Where(x=>x != null);
        }

        public void RaiseSuccessfulLoad(T result)
        {
            _executing = false;
            _resultProperty.Value = new TaskExecutionResult<T>(result, true);
        }

        public void RaiseFailedLoad()
        {
            _executing = false;
            _isFailed = true;
        
            _cancellationTokenSource.Cancel();
            _resultProperty.Value = new TaskExecutionResult<T>(default, false);
        }
        
        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }

        private async UniTaskVoid TryExecuteTask(UniTask<T> taskHandle)
        {
            try {
                // WaitAsync extension method works only with Task and Task<T>!
                var task = taskHandle.AsTask();

                var result = await task.WaitAsync(_cancellationTokenSource.Token);

                if (task.Status != TaskStatus.RanToCompletion || result == null) {
                    RaiseFailedLoad();
                }
                else {
                    RaiseSuccessfulLoad(result);
                }
            }
            catch (OperationCanceledException) {
                RaiseFailedLoad();
            }
            catch (InvalidOperationException) {
                RaiseFailedLoad();
            }
            catch (Exception ex) {
                GameLog.LogError(ex.Message);
                RaiseFailedLoad();
            }
        }
    }
}