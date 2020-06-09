namespace UniModules.UniGame.AsyncUniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;

    public class TaskYieldInstruction : CustomYieldInstruction
    {
        private readonly Task _task;

        public override bool keepWaiting => !_task.IsCompleted;

        public TaskYieldInstruction(Task task)
        {
            _task = task;
        }
    }
    
    public class TaskYieldInstruction<TResult> : IEnumerator<TResult>
    {
        private readonly Task<TResult> _task;
        
        object IEnumerator.Current => Current;
        
        public TResult Current => _task.IsCompleted ? _task.Result : default;

        public TaskYieldInstruction(Task<TResult> task)
        {
            _task = task;
        }

        public void Dispose()
        {
            _task.Dispose();
        }

        public bool MoveNext()
        {
            return !_task.IsCompleted;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}