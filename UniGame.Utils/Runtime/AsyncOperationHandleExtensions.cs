namespace UniGame.Utils.Runtime
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public static class AsyncOperationHandleExtensions
    {
        public static async UniTask<bool> AttachTimeout(this AsyncOperationHandle handle, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (handle.IsDone)
                return false;

            while (cancellationToken.IsCancellationRequested == false)
            {
                var downloadedBytes = handle.GetDownloadStatus().DownloadedBytes;

                var timeoutTask           = UniTask.Delay(timeout, cancellationToken: cancellationToken);
                var bytesCountChangedTask = UniTask.WaitUntil(() => handle.GetDownloadStatus().DownloadedBytes > downloadedBytes, cancellationToken: cancellationToken);
                var taskIndex             = await UniTask.WhenAny(bytesCountChangedTask, timeoutTask);

                if (taskIndex == 1)
                    return true;

                if (handle.IsDone)
                    return false;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            return false;
        }
    }
}