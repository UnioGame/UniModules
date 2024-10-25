# Changelog

## [1.3.7] - 2024-03-21

### Changed
* Re-releasing 1.3.7-pre.1 (prerelease) as 1.3.7 (released)

## [1.3.7-pre.1] - 2024-03-15

### Added
* test: add runtime utility test

### Fixed
* fix-test: use Unity's temporary path to create temp files

### Changed
* test: rename test assembly to Unity.SharpZipLib.* 
* test: move test code copied from source to Tests/Runtime/Src

## [1.3.6-preview] - 2024-02-28

### Fixed
* fix: use SharpZipLib 1.3.3

### Changed
* doc: update the version of SharpZipLib source, DLL build steps, and installation doc

## [1.3.5-preview] - 2024-01-09

### Added
* doc: add "How to Use" section
* doc: add Installation page

## [1.3.4-preview] - 2023-04-07

### Added
* internal: analytics

## [1.3.3-preview] - 2022-12-16

* Reverted minimum Unity version from 2019.4 back to 2018.4 to increase utility

## [1.3.2-preview] - 2022-07-14

* Re-releasing 1.3.1-preview as 1.3.2-preview

## [1.3.1-preview] - 2022-07-13

### Fixed
* fix: exclude github workflows from the package

## [1.3.0-preview] - 2022-05-16

### Changed

* feat: use SharpZipLib 1.3.1
* chore: increase minimum Unity requirement to 2019.4 
* change: the namespace of all code (including DLL) to Unity.SharpZipLib (breaking change)

## [1.2.2-preview.2] - 2021-10-19

### Changed
* change assembly name to Unity.SharpZipLib
* replace SharpZipLib.dll with Unity.SharpZipLib.dll

### Removed
* remove Tests\Runtime\Serialization code

## [1.2.2-preview.1] - 2020-10-02

### Changed
* chore: remove package author.

## [1.2.2-preview] - 2020-06-10

### Fixed
* chore: fix package warnings

## [1.2.1-preview] - 2020-05-12

### Changed
* Changed minimum Unity version from 2019.2 to 2018.4 so it can be used by packages targetting this version


## [1.2.0-preview] - 2019-10-02

### Added
* The first release of *SharpZipLib \<com.unity.sharp-zip-lib\>*, which was built using 
  [SharpZipLib v1.2.0](https://github.com/icsharpcode/SharpZipLib/archive/v1.2.0.zip).

