using System;
using Assets.Scripts.Interfaces;
using UniRx.Async;
using UnityEngine;

namespace Assets.FlipTheKnife.NewPrototype.Scripts.Extension
{
    public static class AsyncExtension
    {

        public static async UniTask WaitUntil(this object source,
            Func<bool> waitFunc)
        {
            while (waitFunc() == false)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        public static async UniTask WaitUntil(this AsyncOperation source)
        {
            while (source.isDone == false)
            {
                await UniTask.DelayFrame(1);
            }

        }

        public static async UniTask WaitUntil(this ICompletionStatus status)
        {
            while (status.IsComplete == false) {
                await UniTask.DelayFrame(1);
            }
        }
    }
}
