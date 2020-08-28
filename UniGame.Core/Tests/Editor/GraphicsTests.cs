namespace UniModules.UniGame.Core.Tests.Editor
{
    using NUnit.Framework;
    using Runtime.Graphics;
    using UnityEngine;

    public class GraphicsTests
    {
        [Test]
        public void AdditiveSumTestRedGreen50()
        {
            var colorA = new Color(0.0f, 1.0f, 0.0f, 0.5f);
            var colorB = new Color(1.0f, 0.0f , 0.0f, 0.5f);
            
            var expectedResult = new Color(0.667f, 0.333f, 0.0f, 0.75f);
            var result         = colorA.AdditiveSum(colorB);
            
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void AdditiveSumTestRedGreen100()
        {
            var colorA = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            var colorB = new Color(1.0f, 0.0f , 0.0f, 1.0f);
            
            var expectedResult = new Color(1.0f, 0.0f , 0.0f, 1.0f);
            var result         = colorA.AdditiveSum(colorB);
            
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void AdditiveSumTestRedGreen1_0()
        {
            var colorA = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            var colorB = new Color(1.0f, 0.0f , 0.0f, 0.0f);
            
            var expectedResult = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            var result         = colorA.AdditiveSum(colorB);
            
            Assert.AreEqual(expectedResult, result);
        }
        
        [Test]
        public void AdditiveSumTestRedGreen0_1()
        {
            var colorA = new Color(0.0f, 1.0f, 0.0f, 0.0f);
            var colorB = new Color(1.0f, 0.0f , 0.0f, 1.0f);
            
            var expectedResult = new Color(1.0f, 0.0f , 0.0f, 1.0f);
            var result         = colorA.AdditiveSum(colorB);
            
            Assert.AreEqual(expectedResult, result);
        }
    }
}