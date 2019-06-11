using NUnit.Framework;

namespace UniNodesTests
{
    using UniGreenModules.UniContextData.Runtime.Entities;
    using UniGreenModules.UniNodeSystem.Runtime;
    using UniGreenModules.UniNodeSystem.Runtime.Connections;

    public class UniPortValueTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void UpdateValueTest()
        {
            
            var portValue = new UniPortValue();
            var context1 = new EntityContext();
            var value1 = "value1";
            var value2 = "value2";
            
            portValue.Add(value1);
            
            var value =portValue.Get<string>();
            
            Assert.That(value,Is.EqualTo(value1));
            
            portValue.Add(value2);

            value = portValue.Get<string>();

            Assert.That(value,Is.EqualTo(value2));
            
        }

        
        [Test]
        public void PortValueRemoveTest()
        {
            var portValue1 = new UniPortValue();

            var testPortValue = "TestPortValue";
            var testPortValue2 = 222;
            
            portValue1.Add(testPortValue);
            portValue1.Add(testPortValue2);

            var result = portValue1.Get<string>();
            var intResult = portValue1.Get<int>();
            
            Assert.That(result,Is.EqualTo(testPortValue));
            Assert.That(intResult,Is.EqualTo(testPortValue2));
        }
        
       
        [Test]
        public void PortConnectionDependenciesTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            var port2Connection = new PortValueConnection(portValue2);

            var testPortValue = "TestPortValue";           
            
            portValue1.Connect(port2Connection);
            portValue1.Add(testPortValue);

            var result = portValue2.Get<string>();
            
            Assert.That(result,Is.EqualTo(testPortValue));
            
        }
        
        [Test]
        public void PortConnectionDependenciesChangesTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            var port2Connection = new PortValueConnection(portValue2);
            
            var context1 = new EntityContext();
            
            var testPortValue = "TestPortValue";
            var testPortValue2 = "TestPortValue2";
            
            portValue1.Connect(port2Connection);
            
            portValue1.Add(testPortValue);
            portValue1.Add(testPortValue2);
            
            Assert.That(portValue1.Get<string>(),Is.EqualTo(testPortValue2));
            Assert.That(portValue2.Get<string>(),Is.EqualTo(testPortValue2));
            
        }
        
        [Test]
        public void PortDependenciesRemoveTest()
        {
            var portValue1 = new UniPortValue();
            var portValue2 = new UniPortValue();
            var port2Connection = new PortValueConnection(portValue2);
            
            var testPortValue = "TestPortValue";
            
            portValue1.Connect(port2Connection);          
            portValue1.Add(testPortValue);

            Assert.That(portValue1.Get<string>(),Is.EqualTo(testPortValue));
            Assert.That(portValue2.Get<string>(),Is.EqualTo(testPortValue));
            
        }
        
        [Test]
        public void PortValueRemoveContextTest()
        {
            var portValue1 = new UniPortValue();

            var testPortValue = "TestPortValue";
            
            portValue1.Add(testPortValue);
            portValue1.Remove<string>();
            
            var result = portValue1.Contains<string>();
            
            Assert.That(result,Is.EqualTo(false));

        }

        [Test]
        public void PortValueRemoveAllTest()
        {
            var portValue1 = new UniPortValue();

            var testPortValue = "TestPortValue";
            var testPortValue2 = 333;
            
            portValue1.Add(testPortValue);
            portValue1.Add(testPortValue2);
            
            portValue1.CleanUp();

            Assert.That(portValue1.Contains<string>(),Is.EqualTo(false));
            Assert.That(portValue1.Contains<int>(),Is.EqualTo(false));
            Assert.That(portValue1.HasValue,Is.EqualTo(false));
        }
        
    }
}
