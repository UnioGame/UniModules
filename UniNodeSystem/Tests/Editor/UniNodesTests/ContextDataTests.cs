namespace UniGreenModules.UniNodeSystem.Tests.Editor.UniNodesTests
{
    using System.Collections;
    using NUnit.Framework;
    using UniContextData.Runtime;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Interfaces;
    using UnityEngine.TestTools;

    public class ContextDataTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ContextDataHasValueOfTypeIntAfterRemove()
        {
            var contextData = new ContextData<IContext>();
            var entity = new EntityContext();
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
