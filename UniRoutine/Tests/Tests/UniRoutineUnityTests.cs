using UnityEngine;

namespace UniGreenModules.UniRoutine.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using UnityEngine.TestTools;
    using UniTools.UniRoutine.Runtime;
    using UniTools.UniRoutine.Runtime.Extension;

    public class UnityRoutineTests
    {
    
        [UnityTest]
        public IEnumerator TimeRoutineTest()
        {
            //arrange
            var items = new List<float>();
            var count = 3;
            var delay = 0.5f;
            var enumerator = AddListByTime(items,count,delay);
            
            //act
            var routine = enumerator.RunWithSubRoutines();
            
            while (routine.IsDisposed == false) {
                yield return null;
            }

            var timePassed = items.Sum();
            Debug.Log($"TimeRoutineTest TIME PASSED : {timePassed}");
            
            //assert
            Assert.That(timePassed > ( count * delay));
            
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
