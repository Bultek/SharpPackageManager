# SharpPackageManager
Package Manager written on C#

## Installation
  * Download Installer: https://github.com/Bultek/SPMinstaller/releases
  * Or you can build it by yourself!
## How to build?
  1. Install .NET 6 SDK
  2. Clone the repo
  3. Build the app
  4.1 Run ```dotnet build```
  4.1.1 Copy the output to C:\SPM
  ##### Alternatively
  4.2 Use ```dotnetpublisher-win.ps1``` from the project path
  4.2.1 Run it with two arguments, 1) Output folder 2) Solution or SharpPackageManager.csproj (we recommend the second option!)
  4.2.2 Copy .exe file to ```C:\SPM```
  ##### Set up config files
  Setup the config files
  5 Create ```sources.txt``` file in ```C:\SPM\config```
  5.1 (optional) Fill the default repo ```bultek, http://bpmr.bultek.com.ua```
  6. Create ```currentversions.txt``` file in ```C:\SPM\config```
  7. Fill previously created file with ```placeholder, 1```
  8. You're good to go!
## Usage
  Usage is pretty obvious, everything is explained when you start the app!
## Contributing
  To contribute to our project you need to
    1. Fork the project
    2. Make some changes in the dev branch
    3. Create a pull request
## Syntax of config files and appkits
   1. Config files
      1.0 apps<reponame>.txt and versions<reponame>.txt are highly connected.
      1.0.1 It means that even if your app won't support built-in update feature, it's name has to be in the versions file
      1.0.2 Also, if your app is in the 17th line in the apps file, it also has to be on the 17th line in the versions file
      1.0.3 apps.txt should be filled with ```app-name, download link```
      1.1 The base of the syntax is ```name, value```
      1.2 If syntax is broken the app WILL crash!
   2. AppKits
      2.0 Just arrange the apps in a txt file (e.g magic.txt)
      2.1 All apps have to be in their own line
      
## How to setup a repo
 1. Use a web hosting (or github pages)
 2. create an apps.txt file on your server
 3. create a versions.txt file on your server
 4. Set up both files using the SPM config syntax
 5. Add the repo to ```C:\SPM\config.txt``` file
 6. You're done!
      
