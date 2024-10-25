# com.unity.sharp-zip-lib

`com.unity.sharp-zip-lib` is a package that wraps [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) to be used inside Unity,
and provides various compression/uncompression utility functions.

Currently, this package uses [SharpZipLib v1.3.3](https://github.com/icsharpcode/SharpZipLib/releases/tag/v1.3.3).  
Please refer to the [installation](Installation.md) page to install this package.
 
> The version numbering of this package itself and the version of SharpZipLib used in the package may look similar, 
but they are not related.


## How to Use

* All [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) APIs are available under `Unity.SharpZipLib` namespace. For example: 
  ```csharp
    using System.IO;
    using Unity.SharpZipLib.GZip;
  
    ...
  
    public void Foo() {
        MemoryStream ms = new MemoryStream();
        GZipOutputStream outStream = new GZipOutputStream(ms);
        ...
    }

  ```

  Please refer to the API documentation of the [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) version used 
  in this package for more details.

* In addition, `com.unity.sharp-zip-lib` also provides additional utility APIs:
  * `ZipUtility.CompressFolderToZip()`: Compresses the files in the nominated folder, and creates a zip file on disk. 
  * `ZipUtility.UncompressFromZip()`: Uncompress the contents of a zip file into the specified folder.
 
  As an example:
  ```csharp
  
  [Test]
  public void Foo() {
      //Compress 
      string tempZipPath = FileUtil.GetUniqueTempPathInProject();
      string folderToCompress ="Bar";
      ZipUtility.CompressFolderToZip(tempZipPath,null, folderToCompress);
  
      //Uncompress
      string tempExtractPath = FileUtil.GetUniqueTempPathInProject();
      Directory.CreateDirectory(tempExtractPath);
      ZipUtility.UncompressFromZip(tempZipPath, null, tempExtractPath);
  
  }
  ```


## Supported Unity Versions

* Unity `2018.4.36` or higher.
