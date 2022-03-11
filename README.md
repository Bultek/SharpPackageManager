# SharpPackageManager
Package Manager written on C#


# Installation/Building

## Installation
  * Download Installer from the [releases tab (recommended)](https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/releases) or download it from [here](https://gitlab.com/bultekdev/spm-projects/SPMinstaller/-/releases)
  * Or you can build it by yourself!
## How to build?
  1. Install .NET 6 SDK
  2. Clone the repo
  3. Open the apps folder in the terminal
  5. Run ```dotnet build```
  6. Copy the output
  7. Now you should organise config files
  8. Create ```sources.txt``` file in ```C:\SPM\config```
  9. (optional) Fill the default repo ```bultek-new, http://repo.bultek.com.ua/spm```
  10. Create ```currentversions.txt``` file in ```C:\SPM\config```
  11. Fill previously created file with ```placeholder, 1```
  12. You're good to go!

# Usage and contributing
## Contributing
  * Request access to the repo (we'll grant it)
  * Clone the repo
  * Do something, idk
## Usage
  Usage is pretty obvious, everything is explained when you start the app!
# Packages and repositories

## How to create a package
 * Create a folder with needed files
 * Create ```AppData.spmdata``` file
 * Fill it using ``config files syntax``
 * There are 3 things that can be modified!
    1. Type (```.exe``` or ```.zip```) (e.g ```type, zip```, ```type, exe```)
    2. Dependencies (e.g ```dep, funnimonkeyframework```)
    3. Executables (Not used in zip type, required in exe type, used as installer) (e.g ```exe, funnimonkeyframeworkinstaller.exe```)
 * Exe type just launches the specified executable (kinda reminds v1.x.x versions)
 * Zip type extracts contents of an archive to ```C:\SPM-APPS\<Package-Name>```
## Syntax of config files and appkits
   * Config files
      1. apps<reponame>.txt and versions<reponame>.txt are highly connected
      2. It means that even if your app won't support built-in update feature, it's name has to be in the versions file
      3. Also, if your app is in the 17th line in the apps file, it also has to be on the 17th line in the versions file
      4. apps.txt should be filled with ```app-name, download link.zip```
      5. The base of the syntax is ```name, value```
      6. If syntax is broken the app WILL have issues!
   * AppKits
      1. Just arrange the apps in a txt file (e.g magic.txt)
      2. All apps have to be in their own line
      
## How to setup a repo
 1. Use a web hosting (or github pages)
 2. create an apps.txt file on your server
 3. create a versions.txt file on your server
 4. Set up both files using the SPM config syntax
 5. Add the repo to ```C:\SPM\sources.txt``` file
 6. You're done!
 
 # Modules
 
 ## WARNING: WE DON'T RECOMMEND USING MODULES, they may cause crashes, compatibility issues (that's why it's not so easy to install them)
 ## How to create a module?
    SPM module is a collection of python scripts
  * Note: we recommend using ```libspm.py``` for os modules
  * Note: Scripts will run under "libspm Python runtime", it means WE DON'T RECOMMEND TO USE ANY CUSTOM MODULES THAT ARE NOT INCLUDED IN PYTHON 3.10.2!
  * We recommend using this [example](https://github.com/mrquantumoff/supersimplebackups-spm-module)
  * Note: Just ```init.py``` file is essential!
 ## How to enable and use modules?
  * Install the ```python310``` package from ```bultek-new``` repo
  * Download libspm.py from the releases tab
  * Create ```C:\SPM\modules``` folder
  * Install modules, it should look like this ```C:\SPM\modules\<module-name>``` 
 
