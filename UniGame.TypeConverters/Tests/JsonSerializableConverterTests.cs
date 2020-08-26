namespace UniModules.UniGame.TypeConverters.Tests
{
    using System.Collections.Generic;
    using Editor;
    using NUnit.Framework;
    using UnityEngine;

    public class JsonSerializableConverterTests
    {
        
        // A Test behaves as an ordinary method
        [Test]
        public void ListAndArraySerializeCheckTest()
        {
            
            //info
            var serializedValue = "[\"1\",\"4\"]";
            var converter       = new JsonSerializableClassConverter();

            //action

            var listResult =converter.TryConvert(serializedValue, typeof(List<int>));
            var listStringResult = converter.TryConvert(serializedValue, typeof(List<string>));
            var arrayInt = converter.TryConvert(serializedValue, typeof(int[]));
            var arrayString = converter.TryConvert(serializedValue, typeof(string[]));
            
            Debug.Log(listResult.result);
            Debug.Log(listStringResult.result);
            Debug.Log(arrayInt.result);
            Debug.Log(arrayString.result);
            
            //check
            Assert.That(listResult.isValid);
            Assert.That(listStringResult.isValid);
            Assert.That(arrayInt.isValid);
            Assert.That(arrayString.isValid);

        }

        // A Test behaves as an ordinary method
        [Test]
        public void ConvertToListAndArraySerializeCheckTest()
        {
            //info
            var converter   = new JsonSerializableClassConverter();
            var targetList  = new List<string>(){"1","4"};
            var targetArray = targetList.ToArray();
            
            //action

            var listResult  =converter.TryConvert(targetList, typeof(string));
            var arrayResult = converter.TryConvert(targetArray, typeof(string));
            
            Debug.Log(listResult.result);
            Debug.Log(arrayResult.result);
            
            //check
            Assert.That(listResult.isValid);
            Assert.That(arrayResult.isValid);

        }
        
    }
}
