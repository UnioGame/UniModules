using UnityEngine;

namespace UniGreenModules.UniRoutine.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using UniCore.Runtime.Interfaces;
    using UnityEngine.TestTools;
    using UniTools.UniRoutine.Runtime;
    using UniTools.UniRoutine.Runtime.Extension;

    public class UnityRoutineTests
    {

        [UnitySetUp]
        public IEnumerator PrepareToTests()
        {
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator TimeRoutineTest()
        {
            //arrange
            var items = new List<float>();
            var count = 3;
            var delay = 0.1f;
            var enumerator = AddListByTime(items,count,delay);
            
            //act
            var routine = enumerator.ExecuteRoutine(RoutineType.Update,true);
            
            yield return new WaitForSeconds(1);

            var timePassed = items.Sum();
            Debug.Log($"TimeRoutineTest TIME PASSED : {timePassed}");
            
            //assert
            Assert.That(timePassed > ( count * delay));
            
        }
        
        [UnityTest]
        public IEnumerator RevertOrderDisposeRoutineTest()
        {
            //arrange
            var counter      = new List<int>(){0};
            var count      = 3;
            var itemsCount = 20;
            var disposable = new List<IDisposable>();
            
            //act
            for (var i = 0; i < itemsCount; i++) {
                var routineDisposable = this.OnUpdate(x => {
                                                          counter[0]++;
                                                          return true;
                                                      }).ExecuteRoutine();
                disposable.Add(routineDisposable);
            }

            yield return null;
            yield return null;

            for (int i = itemsCount-1; i >= 0; i--) {
                var itemDisposable = disposable[i];
                itemDisposable.Dispose();
            }

            //assert
            Assert.That(counter[0] == (itemsCount * 2));
            
        }
        
        [UnityTest]
        public IEnumerator FixedUpdateRoutineTest()
        {
            //arrange
            var counter      = new List<int>();
            counter.Add(0);
            
            IDisposableItem disposableItem = null;
            
            //act
            var routine = this.WaitUntil(() => {
                OnFixedUpdateCounterFunc(counter);
                if (disposableItem == null)
                    return false;
                
                disposableItem.Dispose();
                Debug.Log($"FixedUpdateRoutineTest AFTER DISPOSE");
                return true;
            });

            disposableItem = routine.ExecuteRoutine(RoutineType.FixedUpdate, false);
            
            while (disposableItem.IsDisposed == false) {
                yield return null;
            }

            //assert
            Assert.That(counter[0] == 1);
            
        }

        private void OnFixedUpdateCounterFunc(List<int> counter)
        {
            counter[0]++;
            Debug.Log($"COUNTER TEST {counter[0]}");
        }
        
        private IEnumerator OnFixedUpdateCounter(List<int> counter)
        {
            yield return null;
            counter[0]++;
            yield return null;
            counter[0]++;
            yield return null;
            counter[0]++;
            yield break;
            counter[0]++;
        }
        
        private IEnumerator AddListByTime(List<float> counter, int count,float delay)
        {
            var time = Time.realtimeSinceStartup;
            for (int i = 0; i < count; i++) {
                
                yield return this.WaitForSecondUnscaled(delay);
                
                var activeTime = Time.realtimeSinceStartup;
                var delayedTime = activeTime - time;
                counter.Add(delayedTime);
                time = activeTime;
            }
            
        }
        
    }
}
