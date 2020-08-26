namespace UniModules.UniGame.TypeConverters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Editor;
    using NUnit.Framework;
    using UnityEngine;

    public class TypeConverterTests
    {
        private ObjectTypeConverter converter;
        
        [OneTimeSetUp]
        public void TestSetupOnce()
        {
            converter = ScriptableObject.CreateInstance<ObjectTypeConverter>();
            
            var typeConverters = converter.converters;
                    
            typeConverters.Add(new StringToAssetConverter());
            typeConverters.Add(new StringToAssetReferenceConverter());
            typeConverters.Add(new JsonSerializableClassConverter());
            typeConverters.Add(new StringToPrimitiveTypeConverter());
            typeConverters.Add(new StringToPrimitiveTypeConverter());
        }
        
        [Test]
        public void SelectJsonConvertTest()
        {
            //init
            var sourceValue = new List<string>(){"3","22d"};
            var checkText      = "[\"3\",\"22d\"]";
            
            //action
            var result       = converter.TryConvert(sourceValue, typeof(string));
            var resultString = result.result as string;
            Assert.That(result.isValid);
            Assert.That(result.result is string);
            Assert.That(resultString == checkText);
        }
        
        [Test]
        public void StringToListJsonConvertTest()
        {
            //init
            var sourceValue  = new List<string>(){"3","22d"};
            var sourceString = "[\"3\",\"22d\"]";
            
            //action
            var result       = converter.TryConvert(sourceString, typeof(List<string>));
            var resultValue = result.result as List<string>;
            Assert.That(result.isValid);
            Assert.That(result.result is List<string>);
            Assert.That(resultValue.Count == 2);
            Assert.That(resultValue.SequenceEqual(sourceValue));
        }
        
        [Test]
        public void StringToListIntJsonConvertTest()
        {
            //init
            var sourceValue  = new List<int>(){3,22};
            var sourceString = "[\"3\",\"22\"]";
            
            //action
            var result      = converter.TryConvert(sourceString, typeof(List<int>));
            var resultValue = result.result as List<int>;
            Assert.That(result.isValid);
            Assert.That(result.result is List<int>);
            Assert.That(resultValue.Count == 2);
            Assert.That(resultValue.SequenceEqual(sourceValue));
        }
        
        [Test]
        public void IntToStringConvertTest()
        {
            //init
            var sourceValue = 32;
            var checkText   = "32";
            
            //action
            var result       = converter.TryConvert(sourceValue, typeof(string));
            var resultString = result.result as string;
            Assert.That(result.isValid);
            Assert.That(result.result is string);
            Assert.That(resultString == checkText);
        }
        
        [Test]
        public void StringToIntConvertTest()
        {
            //init
            var sourceValue = 32;
            var sourceString = "32";
            
            //action
            var result       = converter.TryConvert(sourceString, typeof(int));
            var resultValue = (int)result.result;
            Assert.That(result.isValid);
            Assert.That(resultValue == sourceValue);
        }
        
        [Test]
        public void StringDotToFloatConvertTest()
        {
            //init
            var sourceValue  = 3.2f;
            var sourceString = "3.2";
            
            //action
            var result      = converter.TryConvert(sourceString, typeof(float));
            var resultValue = (float)result.result;
            Assert.That(result.isValid);
            Assert.That(Mathf.Approximately(sourceValue,resultValue));
        }
        
        [Test]
        public void StringToFloatConvertTest()
        {
            //init
            var sourceValue  = 3.2f;
            var sourceString = "3,2";
            
            //action
            var result      = converter.TryConvert(sourceString, typeof(float));
            var resultValue = (float)result.result;
            Assert.That(result.isValid);
            Assert.That(Mathf.Approximately(sourceValue,resultValue));
        }
        
        [Test]
        public void FloatToFloatConvertTest()
        {
            //init
            var sourceValue  = 3.2f;
            
            //action
            var result      = converter.TryConvert(sourceValue, typeof(float));
            var resultValue = (float)result.result;
            Assert.That(result.isValid);
            Assert.That(Mathf.Approximately(sourceValue,resultValue));
        }
        
    }
}
