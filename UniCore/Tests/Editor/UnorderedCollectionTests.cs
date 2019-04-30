using UnityEngine;

namespace UniGreenModules.UniCore.Tests
{
    using NUnit.Framework;
    using UniModule.UnityTools.DataStructure;

    public class UnorderedCollectionTests
    {
        
        // A Test behaves as an ordinary method
        [Test]
        public void AddElementsIntoCollection()
        {
            //info
            var items = new UnorderedCollection<object>();
            
            //action
            items.Add(new object());
            items.Add(new object());
            items.Add(new object());
            
            Assert.That(items.Count == 3);
        }
        
        [Test]
        public void EmptyCollection()
        {
            var items = new UnorderedCollection<object>();
            Assert.That(items.Count == 0);
        }
        
        [Test]
        public void AddAndRemoveCollection()
        {
            var items = new UnorderedCollection<object>();
            
            //action
            items.Add(new object());
            items.Add(new object());
            var id = items.Add(new object());
            items.Remove(id);
            
            Assert.That(items.Count == 2);
        }
        
        [Test]
        public void RemoveAndIterateCollection()
        {
            var items = new UnorderedCollection<object>();
            var target = "zero";
            
            //action
            var id = items.Add(target);
            items.Add("one");
            items.Add("two");
            items.Remove(id);

            foreach (var item in items.GetItems()) {
                Assert.That(item != target);
            }
            
        }
        
    }
}
