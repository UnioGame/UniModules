using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    using UniGreenModules.UniContextData.Runtime;
    using UniGreenModules.UniContextData.Runtime.Entities;

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
