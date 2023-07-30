#if UNITY_2019_1_OR_NEWER

using System.IO;
using NUnit.Framework;
using UnityEditor;

namespace Unity.SharpZipLib.Utils.EditorTests {

    public class ZipUtilityEditorTests {
    [Test]
    public void CompressAndDecompress() {
        //Compress code
        string tempZipPath = FileUtil.GetUniqueTempPathInProject();
        string runtimeSrcPath ="Packages/com.unity.sharp-zip-lib/Runtime";
        ZipUtility.CompressFolderToZip(tempZipPath,null, runtimeSrcPath);
        Assert.True(File.Exists(tempZipPath));

        //Uncompress
        string tempExtractPath = FileUtil.GetUniqueTempPathInProject();
        Directory.CreateDirectory(tempExtractPath);
        ZipUtility.UncompressFromZip(tempZipPath, null, tempExtractPath);

        string[] extractedFiles = Directory.GetFiles(tempExtractPath);
        Assert.Greater(extractedFiles.Length,0);

        //Cleanup
        Directory.Delete(tempExtractPath,true);
        File.Delete(tempZipPath);
    }

}

} //end namepsace

#endif
