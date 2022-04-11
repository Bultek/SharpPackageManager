# SharpPackageManager
Package Manager written on C#

#### Status: [![Code Quality Rank](https://app.codacy.com/project/badge/Grade/54a8a31a08604afeaee09e1852919214)](https://www.codacy.com/gl/bultekdev/SharpPackageManager/dashboard?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=bultekdev/spm-projects/SharpPackageManager&amp;utm_campaign=Badge_Grade)   [![pipeline status](https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/badges/dev/pipeline.svg)](https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/commits/dev)   [![Latest Release](https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/badges/release.svg)](https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/releases) 

# Installation, Building and Developing

## Installation
  * Download Installer from the [releases tab (recommended)](https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/releases) or download it from [here](https://gitlab.com/bultekdev/spm-projects/SPMinstaller/-/releases)
  * Or you can build it by yourself!
## How to build
  1. Install Visual Studio 2022 Community with .NET Desktop Development Pack
  2. Install .NET 6 SDK
  3. Clone the repo
  4. Open The Repo in ```Developer Command Prompt for VS 2022```
  5. Run ```msbuild```
  6. Copy the output (```bin``` folder)
  7. Now you should organise config files
  8. Create ```sources.txt``` file in ```C:\SPM\config```
  9. (optional) Fill the default repo ```bultek-new, http://repo.bultek.com.ua/spm/apps.txt```
  9. (alternative) Download the ```mirrorlist-bultek``` package [from here]( http://repo.bultek.com.ua/spm/mirrorlist-bultek.zip ) and unzip mirrorlist-bultek.txt file from it and set the !MIRRORLIST file in the ```C:\SPM\config\sources.txt``` file
  10. Create ```currentversions.txt``` file in ```C:\SPM\config```
  11. Fill previously created file with ```placeholder, 1```
  12. You're good to go!
## Additional Development Notes
  1. You can try to debug the app using VSCode/Other IDEs/Code Editors, but we recommend to debug SPM using [Visual Studio Community 2022](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release)

# Usage and contributing
## Contributing
  * Request access to the repo (we'll grant it)
  * Clone the repo
  * Do something in the dev branch
  * Create a merge request to ```dev-testing``` branch
## Usage
  Usage is pretty obvious, everything is explained when you start the app!
# Packages and repositories

## How to create a package
 * Create a folder with needed files
 * Create ```AppData.spmdata``` file
 * Fill it using ``config files syntax``
 * There are 4 things that can be modified!
    1. Type (```.exe```, ```.zip``` and ```configfile```) (e.g ```type, zip```, ```type, exe```)
    2. Dependencies (e.g ```dep, funnimonkeyframework```)
    3. Executables/Config files (Optional in zip type as shortcut paths (Only one shortcut can be created for now, we will improve this in new versions), required in exe type, used as installer) (e.g ```exe, C:\SPM-APPS\funnimonkeyframework\funnimonkeyframeworkinstaller.exe``` or ```C:\SPM-APPS\funnimonkey-mirrorlist\funnimonkeymirrorlist.txt```)
    4. Even though ```exe``` option exists in ```configfile``` type, it's being used as the config file which will be copied to ```C:\SPM\config\``` folder.
 * ```exe``` type just launches the specified executable (kinda reminds v1.x.x versions)
 * ```zip``` type extracts contents of the archive to ```C:\SPM-APPS\<Package-Name>```
 * ```configfile``` type copies speciefied files to ```C:\SPM\config\```
## Syntax of config files and appkits
   * Config files
      1. apps<reponame>.txt and versions<reponame>.txt are highly connected
      2. It means that even if your app won't support built-in update feature, it's name has to be in the versions file
      3. Also, if your app is in the 17th line in the apps file, it also has to be on the 17th line in the versions file
      4. apps.txt should be filled with ```app-name, download link.zip```
      5. The base of the syntax is ```name, value```
      6. If syntax is broken the app WILL have issues!
      7. Since v2.4.0 - you can add ```!MIRRORLIST=*mirrorlist file.txt*``` to sources.txt
      8. Since v2.4.0 - you can add !MIRRORURL</yourpackage.zip> to apps.txt file on your server. This will tell the SPM client that you have a mirror for this package.
   * AppKits and mirrorlists
      1. Just arrange the apps/mirrors in a txt file (e.g magic.txt)
      2. All apps have to be in their own line
      
## How to setup a repo
 1. Use a web hosting (or github pages)
 2. create an apps.txt file on your server
 3. create a versions.txt file on your server
 4. Set up both files using the SPM config syntax
 5. Add the repo to ```C:\SPM\config\sources.txt``` file
 6. The repo should end with ```/apps.txt``` in the ```sources.txt``` file
 7. You're done!
 
 # Modules
 
 ## WARNING: WE DON'T RECOMMEND USING MODULES, they may cause crashes, compatibility issues (that's why it's not so easy to install them)
 ## How to create a module
    SPM module is a collection of python scripts
  * Note: we recommend using ```libspm.py``` for os modules
  * Note: Scripts will run under "libspm Python runtime", it means WE DON'T RECOMMEND TO USE ANY CUSTOM MODULES THAT ARE NOT INCLUDED IN PYTHON 3.10.2!
  * We recommend using this [example](https://github.com/mrquantumoff/supersimplebackups-spm-module)
  * Note: Just ```init.py``` file is essential!
 ## How to enable and use modules
  * Install the ```python310``` package from ```bultek-new``` repo
  * Download libspm.py from the releases tab
  * Create ```C:\SPM\modules``` folder
  * Install modules, it should look like this ```C:\SPM\modules\<module-name>``` 
 
# Additional Notes
  * The list of projects that are officialy supported by us or affilated with SPM is [here](https://gitlab.com/bultekdev/spm-projects), the only exception is the [bultek-new](https://github.com/Bultek/bultek-new-spm-repo) SPM repository