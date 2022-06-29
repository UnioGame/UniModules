namespace UniGame.Utils.Runtime
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ProfilerTools;

    public static class SemaphoreExtensions
    {
        public static async UniTask WaitAsyncWithAction(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken, Action action)
        {
            try
            {
                await semaphoreSlim.WaitAsync(cancellationToken);
                
                if(cancellationToken.IsCancellationRequested)
                    return;
                
                action?.Invoke();
            }
            catch (OperationCanceledException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (ArgumentNullException ex)
            {
                GameLog.LogWarning($"{nameof(ArgumentNullException)}: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                try
                {
                    semaphoreSlim.Release();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        public static async UniTask WaitAsyncWithAsyncAction(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken, Func<UniTask> action)
        {
            try
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return;

                await action.Invoke();
            }
            catch (OperationCanceledException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (ArgumentNullException ex)
            {
                GameLog.LogWarning($"{nameof(ArgumentNullException)}: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                try
                {
                    semaphoreSlim.Release();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }
    }
}