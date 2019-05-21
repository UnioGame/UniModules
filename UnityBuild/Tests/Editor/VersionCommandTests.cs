using UnityEngine;

namespace UniGreenModules.UnityBuild.Tests
{
    using Editor.ClientBuild.Commands.PreBuildCommands;
    using NUnit.Framework;
    using UnityEditor;

    [TestFixture]
    public class VersionCommandTests : MonoBehaviour
    {

        [Test]
        public void GetAndroidVersionTest()
        {
            
            //arrange

            var buildHandler = new BuildVersionProvider();
            var buildNumber = 100;
            
            //act
            var version = buildHandler.GetBuildVersion(BuildTarget.Android,"1.0.2", buildNumber);

            //assert
            Assert.That(version,Is.EqualTo("1.0.2.100"));            

        }
        
        [Test]
        public void GetIosVersionTest()
        {
            
            //arrange

            var buildHandler = new BuildVersionProvider();
            var buildNumber  = 100;
            
            //act
            var version = buildHandler.GetBuildVersion(BuildTarget.iOS,"1.0.2", buildNumber);

            //assert
            Assert.That(version,Is.EqualTo("1.0.100"));            

        }
        
    }
}
