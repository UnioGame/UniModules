#if UNITY_2019_1_OR_NEWER

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace Unity.SharpZipLib.Utils.Tests {

public class ZipUtilityTests {
        
    [Test]
    public void CompressAndUncompressFolder() {

        Dictionary<string, string> fileContentDictionary = new Dictionary<string, string>() {
            {"Foo.txt", "FooFoo"},
            {"Bar.txt", "BarBar"}
        };

        string folder = Path.Combine(Application.temporaryCachePath, "TestDataFolder");
        Directory.CreateDirectory(folder);
        using (var enumerator = fileContentDictionary.GetEnumerator()) {
            while (enumerator.MoveNext()) {
                KeyValuePair<string, string> kv = enumerator.Current;
                string filePath = Path.Combine(folder, kv.Key);
                File.WriteAllText(filePath, kv.Value);
                Assert.True(File.Exists(filePath));
            }
        }
        
        //Compress folder
        string tempZipPath = Path.Combine(Application.temporaryCachePath, "TestDataFolder.zip");
        ZipUtility.CompressFolderToZip(tempZipPath,null, folder);
        Assert.True(File.Exists(tempZipPath));
        
        Directory.Delete(folder, recursive:true);
        
        
        //Uncompress
        ZipUtility.UncompressFromZip(tempZipPath, null, folder);

        //Check contents
        using (var enumerator = fileContentDictionary.GetEnumerator()) {
            while (enumerator.MoveNext()) {
                KeyValuePair<string, string> kv = enumerator.Current;
                string filePath = Path.Combine(folder, kv.Key);
                Assert.True(File.Exists(filePath));
                Assert.AreEqual(kv.Value, File.ReadAllText(filePath));
            }
        }
        
        //Cleanup
        Directory.Delete(folder,true);
        File.Delete(tempZipPath);
    }

}

} //end namepsace

#endif
