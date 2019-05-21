namespace UniGreenModules.UnityBuild.Tests
{
    using Editor.ClientBuild.Commands.PreBuildCommands;
    using NUnit.Framework;
    using UnityEditor;
    using UnityEditor.VersionControl;

    [TestFixture]
    public class UpdateVersionBuildCommand 
    {
        [Test]
        public void UpdateAndroidVersionTest()
        {
            //arrange
            var versionProvider = new BuildVersionProvider();
            var buildNumber = 100;

            //act
            var version = versionProvider.GetBuildVersion(BuildTarget.Android, "1.0.2", buildNumber);

            //assert
            Assert.That(version,Is.EqualTo("1.0.2.100"));
        }

        [Test]
        public void UpdateIosVersionTest()
        {
            //arrange
            var versionProvider = new BuildVersionProvider();
            var buildNumber     = 100;

            //act
            var version = versionProvider.GetBuildVersion(BuildTarget.iOS, "1.0.2", buildNumber);

            //assert
            Assert.That(version,Is.EqualTo("1.0.100"));
        }
        
    }
}
