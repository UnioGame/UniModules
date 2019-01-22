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
        public void PortValueCopyTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            
            var context1 = new EntityObject();
            var context2 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            var testPortValue2 = "TestPortValue";
            
            portValue1.UpdateValue(context1,testPortValue);
            portValue2.UpdateValue(context2,testPortValue2);
            
            var writer = portValue2.GetPublisher(context1);
            var writer2 = portValue2.GetPublisher(context2);
            
            portValue1.CopyTo(context1,writer);
            portValue1.CopyTo(context2,writer2);
            
            Assert.That(portValue2.Get<string>(context1),Is.EqualTo(testPortValue));
            Assert.That(portValue2.Get<string>(context2),Is.EqualTo(testPortValue2));
            
        }
        
        [Test]
        public void PortValueCopyToExistsTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            
            var context1 = new EntityObject();
            
            var testPortValue2 = "TestPortValue22";
            var testPortValue3 = "TestPortValue33";
            var testPortValue4 = 444;
                        
            portValue1.UpdateValue(context1,testPortValue2);
            portValue1.UpdateValue(context1,testPortValue4);

            portValue2.UpdateValue(context1,testPortValue3);

            var writer2 = portValue2.GetPublisher(context1);
            portValue1.CopyTo(context1,writer2);
            
            Assert.That(portValue2.Get<string>(context1),Is.EqualTo(testPortValue2));
            Assert.That(portValue2.Get<int>(context1),Is.EqualTo(testPortValue4));
        }
        
        [Test]
        public void PortValueRemoveTest()
        {
            var portValue1 = new UniPortValue();
            var context1 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            var testPortValue2 = 222;
            
            portValue1.UpdateValue(context1,testPortValue);
            portValue1.UpdateValue(context1,testPortValue2);
            var result = portValue1.Remove<string>(context1);
           
            Assert.That(result,Is.EqualTo(true));
            Assert.That(portValue1.HasValue(context1,typeof(string)),Is.EqualTo(false));
            Assert.That(portValue1.HasValue<string>(context1),Is.EqualTo(false));
            
        }
        
        [Test]
        public void PortValueRemoveContextTest()
        {
            var portValue1 = new UniPortValue();
            var context1 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            var testPortValue2 = 222;
            
            portValue1.UpdateValue(context1,testPortValue);
            portValue1.UpdateValue(context1,testPortValue2);
            var result = portValue1.RemoveContext(context1);
           
            Assert.That(result,Is.EqualTo(true));
            Assert.That(portValue1.HasValue(context1,typeof(string)),Is.EqualTo(false));
            Assert.That(portValue1.HasValue<string>(context1),Is.EqualTo(false));
            
        }
        
        [Test]
        public void PortValueRemoveNullContextTest()
        {
        
            var portValue1 = new UniPortValue();
            var context1 = new EntityObject();
                     
            var result = portValue1.RemoveContext(context1);          
            Assert.That(result,Is.EqualTo(false));

        }
    }
}
