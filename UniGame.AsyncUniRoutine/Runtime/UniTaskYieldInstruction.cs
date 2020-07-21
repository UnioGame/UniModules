namespace UniModules.UniGame.AsyncUniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Core.Runtime.Extension;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class UniTaskYieldInstruction : CustomYieldInstruction
    {
        private readonly UniTask _task;

        public override bool keepWaiting => !_task.IsCompleted();

        public UniTaskYieldInstruction(UniTask task)
        {
            _task = task;
        }
    }

    public class UniTaskYieldInstruction<TResult> : IEnumerator<TResult>
    {
        private readonly UniTask _task;
        private IEnumerator _taskRoutine;
        private TResult _result;

        public TResult Current => _task.IsCompleted() ? 
            _result : default;

        object IEnumerator.Current => Current;

        public UniTaskYieldInstruction(UniTask<TResult> task)
        {
            _taskRoutine = task.ToCoroutine(x => _result = x);
            _task = task;
        }

        public bool MoveNext()
        {
            return _taskRoutine.MoveNext();
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