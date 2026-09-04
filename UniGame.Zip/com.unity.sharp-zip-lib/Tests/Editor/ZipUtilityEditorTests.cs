#if UNITY_2019_1_OR_NEWER

using System.IO;
using NUnit.Framework;
using UnityEditor;

namespace Unity.SharpZipLib.Utils.EditorTests {

    public class ZipUtilityEditorTests {
    [Test]
    public void CompressAndDecompress() {
        string tempZipPath = FileUtil.GetUniqueTempPathInProject();
        string tempSourcePath = FileUtil.GetUniqueTempPathInProject();
        string tempExtractPath = FileUtil.GetUniqueTempPathInProject();
        const string fileName = "round-trip.txt";
        const string contents = "known zip contents";

        try {
            Directory.CreateDirectory(tempSourcePath);
            File.WriteAllText(Path.Combine(tempSourcePath, fileName), contents);

            ZipUtility.CompressFolderToZip(tempZipPath, null, tempSourcePath);
            Assert.True(File.Exists(tempZipPath));

            ZipUtility.UncompressFromZip(tempZipPath, null, tempExtractPath);

            string extractedFilePath = Path.Combine(tempExtractPath, fileName);
            Assert.True(File.Exists(extractedFilePath));
            Assert.AreEqual(contents, File.ReadAllText(extractedFilePath));
        }
        finally {
            if (Directory.Exists(tempSourcePath))
                Directory.Delete(tempSourcePath, true);
            if (Directory.Exists(tempExtractPath))
                Directory.Delete(tempExtractPath, true);
            if (File.Exists(tempZipPath))
                File.Delete(tempZipPath);
        }
    }

}

} //end namepsace

#endif
