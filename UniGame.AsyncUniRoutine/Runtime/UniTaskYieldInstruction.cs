namespace UniModules.UniGame.AsyncUniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniRx.Async;
    using UnityEngine;

    public class UniTaskYieldInstruction : CustomYieldInstruction
    {
        private readonly UniTask _task;

        public override bool keepWaiting => !_task.IsCompleted;

        public UniTaskYieldInstruction(UniTask task)
        {
            _task = task;
        }
    }

    public class UniTaskYieldInstruction<TResult> : IEnumerator<TResult>
    {
        private readonly UniTask<TResult> _task;

        public TResult Current => _task.IsCompleted ? _task.Result : default;

        object IEnumerator.Current => Current;

        public UniTaskYieldInstruction(UniTask<TResult> task)
        {
            _task = task;
        }

        public bool MoveNext()
        {
            return !_task.IsCompleted;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            throw new NotSupportedException();
        }
    }
}