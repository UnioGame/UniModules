using System.Collections;
using NUnit.Framework;
using UniStateMachine.Nodes;
using UnityEditor.Build.Pipeline;
using UnityEngine.TestTools;
using UnityTools.ActorEntityModel;

namespace UniNodesTests
{
    public class UniPortValueTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void UpdateValueTest()
        {
            
            var portValue = new UniPortValue();
            var context1 = new EntityObject();
            var value1 = "value1";
            var value2 = "value2";
            
            portValue.UpdateValue(context1,value1);
            
            var value =portValue.Get<string>(context1);
            
            Assert.That(value,Is.EqualTo(value1));
            
            portValue.UpdateValue(context1,value2);

            value = portValue.Get<string>(context1);

            Assert.That(value,Is.EqualTo(value2));
            
        }

        [Test]
        public void PortSubscriptionClassTest()
        {
            var portValue = new UniPortValue();
            var context1 = new EntityObject();
            
            var callbackValue = string.Empty;
            var testPortValue = "TestPortValue";
            
            var disposable = portValue.Subscribe<string>(context1, x => { callbackValue = x;});
            
            portValue.UpdateValue(context1,testPortValue);
            
            Assert.That(callbackValue,Is.EqualTo(testPortValue));
            
            disposable.Dispose();
        }

        [Test]
        public void PortSubscriptionValueTest()
        {
            var portValue = new UniPortValue();
            var context1 = new EntityObject();
            
            var callbackValue = 0;
            var testPortValue = 1111;
            
            var disposable = portValue.Subscribe<int>(context1, x => { callbackValue = x;});
            
            portValue.UpdateValue(context1,testPortValue);
            
            Assert.That(callbackValue,Is.EqualTo(testPortValue));
            
            disposable.Dispose();
        }
        
        [Test]
        public void PortNullValueTest()
        {
            var portValue = new UniPortValue();
            var context1 = new EntityObject();
            
            var stringCallbackValue = string.Empty;
   
            var testPortValue = "TestPortValue";
            
            var disposable = portValue.
                Subscribe<string>(context1, x => { stringCallbackValue = x;});

            portValue.UpdateValue(context1,testPortValue);

            Assert.That(stringCallbackValue,Is.EqualTo(testPortValue));
            
            portValue.Remove<string>(context1);
            
            Assert.That(stringCallbackValue,Is.EqualTo(null));
                        
            disposable.Dispose();
        }
        
    }
}
