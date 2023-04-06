
# Latest official docs
- [English](https://docs.unity3d.com/Packages/com.unity.sharp-zip-lib@latest)
 
[![](https://badge-proxy.cds.internal.unity3d.com/78081939-b2a0-4fc9-bebf-901b49fd954c)](https://badges.cds.internal.unity3d.com/packages/com.unity.sharp-zip-lib/build-info?branch=master&testWorkflow=package-isolation)
[![](https://badge-proxy.cds.internal.unity3d.com/faf61743-4d21-479c-b01c-ab63561e27d9)](https://badges.cds.internal.unity3d.com/packages/com.unity.sharp-zip-lib/dependencies-info?branch=master&testWorkflow=updated-dependencies)
[![](https://badge-proxy.cds.internal.unity3d.com/e5917bce-0357-4f49-a9c3-c356b9de832c)](https://badges.cds.internal.unity3d.com/packages/com.unity.sharp-zip-lib/dependants-info)
[![](https://badge-proxy.cds.internal.unity3d.com/f2066c51-3423-424d-a58f-24a64683cf57)](https://badges.cds.internal.unity3d.com/packages/com.unity.sharp-zip-lib/warnings-info?branch=master)

![ReleaseBadge](https://badge-proxy.cds.internal.unity3d.com/3b04c8fe-9005-4b46-848e-cb0199e49a2e)
![ReleaseBadge](https://badge-proxy.cds.internal.unity3d.com/9a481f99-fa9b-4716-8409-69bb63fedbd7)
# com.unity.sharp-zip-lib

`com.unity.sharp-zip-lib` is a package that wraps [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) to be used inside Unity,
and provides various compression/extraction utility functions.

Currently, this packages uses [SharpZipLib v1.3.1](https://github.com/icsharpcode/SharpZipLib/releases/tag/v1.3.1).
 
> The version numbering of this package itself and the version of SharpZipLib used in the package may look similar, 
but they are not related.
EOF          
## Steps to update SharpZipLib

### Windows

#### Requirements
1. Visual Studio 2019 (16.11.13) or later
   * Check ".Net desktop development"

#### Steps

1. Download the source from https://github.com/icsharpcode/SharpZipLib/releases
1. Extract the source into "SharpZipLibSrc" folder
1. Open Developer Command Prompt for Visual Studio
1. Execute `update_sharp-zip-lib.cmd`
1. Open SharpZipLib~ test project, ensure everything compiles and the tests are successful



*Auto-generated on Wed Jul  6 14:13:09 UTC 2022*
