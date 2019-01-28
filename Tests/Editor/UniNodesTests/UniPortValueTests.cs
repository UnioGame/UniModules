using NUnit.Framework;
using UniModule.UnityTools.ActorEntityModel;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UniStateMachine.Nodes;

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
        public void PortDependenciesTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            
            var context1 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            
            portValue1.Add(portValue2);
            
            portValue1.UpdateValue(context1,testPortValue);

            Assert.That(portValue1.Get<string>(context1),Is.EqualTo(testPortValue));
            Assert.That(portValue2.Get<string>(context1),Is.EqualTo(testPortValue));
            
        }
        
        [Test]
        public void PortConnectionDependenciesTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            var port2Connection = new PortValueConnection(portValue2);
            
            var context1 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            
            portValue1.Add(port2Connection);
            
            portValue1.UpdateValue(context1,testPortValue);

            Assert.That(portValue1.Get<string>(context1),Is.EqualTo(testPortValue));
            Assert.That(portValue2.Get<string>(context1),Is.EqualTo(testPortValue));
            
        }
        
        [Test]
        public void PortConnectionDependenciesChangesTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            var port2Connection = new PortValueConnection(portValue2);
            
            var context1 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            var testPortValue2 = "TestPortValue2";
            
            portValue1.Add(port2Connection);
            
            portValue1.UpdateValue(context1,testPortValue);
            portValue1.UpdateValue(context1,testPortValue2);

            Assert.That(portValue1.Get<string>(context1),Is.EqualTo(testPortValue2));
            Assert.That(portValue2.Get<string>(context1),Is.EqualTo(testPortValue2));
            
        }
        
        [Test]
        public void PortDependenciesRemoveTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            var port2Connection = new PortValueConnection(portValue2);
            
            var context1 = new EntityObject();
            
            var testPortValue = "TestPortValue";
            
            portValue1.Add(port2Connection);
            
            portValue1.UpdateValue(context1,testPortValue);

            Assert.That(portValue1.Get<string>(context1),Is.EqualTo(testPortValue));
            Assert.That(portValue2.Get<string>(context1),Is.EqualTo(testPortValue));
            
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
