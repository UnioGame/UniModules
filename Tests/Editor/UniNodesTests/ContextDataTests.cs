using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTools.ActorEntityModel;

namespace Tests
{
    public class ContextDataTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ContextDataHasValueOfTypeIntAfterRemove()
        {
            var contextData = new ContextData<IContext>();
            var entity = new EntityObject();
            var value = 666;
            
            contextData.UpdateValue(entity,value);
            var result = contextData.Get<int>(entity);
            
            Assert.That(result,Is.EqualTo(value));
            
            contextData.Remove<int>(entity);
            
            Assert.That(contextData.HasValue<int>(entity),Is.False);
        }

        
        
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        [Ignore("Demo")]
        public IEnumerator ContextDataTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
