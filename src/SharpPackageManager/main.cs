// Some parts of this code were made by GitHub Copilot. If any of this code violates license of other project, most probably it's written by Github Copilot. If you have any questions or concerns contact GitHub and Microsoft.
// This Code is licensed under the BSD 2 Clause License.
// Copyright (c) 2022, BultekDev / SharpPackageManager
// All rights reserved.

using IWshRuntimeLibrary;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
#pragma warning disable SYSLIB0014,CS4014,CS8618 // I don't care about this warnings.

public static class SharpPackageManager
{
    public const string StartMenuDirectory = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\SPM-APPS";
    public static bool AreModulesLoaded = false;
    public static readonly int currentversion = 34;
    public static readonly string date = DateTime.Now.ToString("dd-MM"); // needed for an easter egg
    public static readonly string appversion = "v2.4.1 - Testing build ID " + currentversion;
    public static readonly string codename = "berg";
    public static readonly string curbranch = "master";

    // If this is changed, please check the release notes in the releases tab
    public const float currentapiversion = 2.4f;


    private static List<String> reponames = new List<String>();
    private static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();
    public static List<String> updateappnames = new List<String>();
    public static List<int> updateversions = new List<int>();
    public static List<String> currentappnames = new List<String>();
    public static List<int> currentappversions = new List<int>();
    public const string InstallDir = "C:\\SPM\\config\\";
    public const string InstallPath = "C:\\SPM\\";
    private static List<String> exectuable = new List<String>();

    private static List<String> shortcuts = new List<String>();
    private static List<String> type = new List<String>();
    private static Dictionary<string, string> repos = new Dictionary<string, string>();

    public static void Main(string[] args)
    {
        // It may fix some issues
        MAIN(args);
    }
    public static void MAIN(string[] args, bool launchmainapp = true, bool output = true, bool forexit = false)
    {
        // Sometimes may be useful
        Debug.WriteLine(currentversion);
        if (output)
        {
            Console.Title = "SharpPackageManager";
            Debug.WriteLine(Console.LargestWindowWidth + "x" + Console.LargestWindowHeight);
        }

        // If the app was upgraded with old SPM versions (if it's possible ofc)
        if (System.IO.File.Exists(@"C:\\SPM\\futureversion\\"))
        {
            DataUpdate(true);
            CheckForAppUpdates(true, true, output);
            DataLoad(InstallDir + "currentversions.txt", "currentversions");
            if (!currentappnames.Contains("spmupdatemanager"))
            {
                Console.WriteLine("================================");
                Console.WriteLine("Installing SPM Update Manager...");
                Console.WriteLine("================================");
                InstallPkg("spmupdatemanager", true, false, false);
            }
            Process PackageStartInfo = new Process();
            PackageStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\spmupdatemanager\\SharpPackageManagerUpdateUtility.exe";
            PackageStartInfo.StartInfo.Arguments = curbranch + " " + currentversion;
            PackageStartInfo.StartInfo.UseShellExecute = true;
            PackageStartInfo.Start();
            System.Environment.Exit(0);
        }
        else
        {
            MainApp(args, output);
        }

    }
    public static void MainApp(string[] args, bool output = true)
    {
        if (output && date == "01-04" || args.Contains("--rickrollme"))
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("It's the rickroll day!");
            Console.WriteLine("Never gonna give you up,\n never gonna let you down,\n never gonna run around and desert you,\n never gonna make you cry,\n never gonna say goodbye,\n never gonna tell a lie and hurt you.\n");
            Console.WriteLine("==============================================");
        }
        string action = "null"; // Some placeholder
        bool argav = false; // Are args available
        Debug.WriteLine("Loading Modules");
        if (System.IO.File.Exists(@"C:\SPM\libspm.py") && Directory.Exists(@"C:\SPM-APPS\python310") && Directory.Exists(@"C:\SPM\modules") && !AreModulesLoaded)
        {
            modules = Directory.GetDirectories(@"C:\SPM\modules");
            foreach (string module in modules)
            {
                if (System.IO.File.Exists(module + "\\libspm.py"))
                {
                    System.IO.File.Delete(module + "\\libspm.py");
                }
                System.IO.File.Copy("C:\\SPM\\libspm.py", module + "\\libspm.py");
                Process PackageStartInfo = new Process();
                PackageStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\spmupdatemanager\\SharpPackageManagerUpdateUtility.exe";
                PackageStartInfo.StartInfo.Arguments = module + "\\init.py";
                PackageStartInfo.StartInfo.UseShellExecute = true;
                PackageStartInfo.Start();
                Debug.WriteLine(module + " Should be loaded");
                Console.Clear();
                AreModulesLoaded = true;
            }
            Debug.WriteLine("Are Modules Loaded: " + AreModulesLoaded);
        }
        else Debug.WriteLine("No modules are loaded");

        // Set up some core dirs
        if (!System.IO.Directory.Exists("C:\\SPM\\Downloads")) System.IO.Directory.CreateDirectory("C:\\SPM\\Downloads");
        if (!System.IO.Directory.Exists("C:\\SPM\\config")) System.IO.Directory.CreateDirectory("C:\\SPM\\config");
        // you can't proceed without currentversions.txt
        if (!System.IO.File.Exists(InstallDir + "currentversions.txt"))
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("ERROR: Can't find currentversions.txt, please create it and set it up!");
            Console.WriteLine("Opening instructions in your default browser...");
            Console.WriteLine("==============================================");
            string upbranch;
            if (curbranch == "ptb") upbranch = "ptb";
            else upbranch = "stable";
            Process.Start("explorer", "https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/blob/"+upbranch+"/README.md#syntax-of-config-files-and-appkits");
            Process.Start("explorer", @"C:\SPM\config");
            PressAnyKey("exit", true, 1);
        }
        // Load data
        DataLoad(InstallDir + "sources.txt", "repos");
        DataLoad(InstallDir + "currentversions.txt", "currentversions");
        if (System.IO.File.Exists(InstallDir + "latestversions.txt")) DataLoad(InstallDir + "latestversions.txt", "updates");

        foreach (string repo in reponames)
        {
            if (System.IO.File.Exists(InstallDir + "apps" + repo + ".txt")) DataLoad(InstallDir + "apps" + repo + ".txt", "apps");
        }
        if (args.Length == 0 && output)
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine("Sharp Package Manager by Bultek. " + appversion);
            Console.WriteLine("Sharp Package Manager API compatibility Version: " + currentapiversion);
            Console.WriteLine("Note: We recommend updating database!");
            Console.WriteLine("Note: We don't recommend using long commands in interactive mode!");
            Console.WriteLine("Please choose your action!");
            Console.WriteLine("================================================================================\n");
            Console.WriteLine("Install a package (Command: i, install)");
            Console.WriteLine("Install an AppKit (Command: ak, appkit)");
            Console.WriteLine("Update database (Command: up, update)");
            Console.WriteLine("Check for SPM updates (Command: spmup)");
            Console.WriteLine("Check for app updates and upgrade packages (Command: upg, upgrade)");
            Console.WriteLine("Search for packages (Command: se, search)");
            Console.WriteLine("Remove a package (Works only with .zip type packages. Command: remove)");
            Console.WriteLine("Add SPM to path (Command: pathadd)");
            Console.WriteLine("Clean up (Command: cleanup)");
            Console.WriteLine("List Packages (listall/listinstalled)");
            Console.WriteLine("================================================================================ \n");
            action = Console.ReadLine(); 
        }
        else if (args.Length > 0)
        {
            action = args[0];
            argav = true;
        }

        if (action == "i" || action == "install")
        {
            if (!argav)
            {
                Console.WriteLine("================================================================================");
                Console.WriteLine("Package to install (note: you can install only one package)");
                string Package = Console.ReadLine();
                if (Package != null)
                {
                    InstallPkg(Package, false, false, true);
                    Console.WriteLine("================================================================================");
                }

                else { Console.WriteLine("ERROR: Package can't be null"); PressAnyKey("exit", true); }
            }
            else
            {
                if (args.Length == 2)
                {
                    Console.WriteLine("================================================================================");
                    InstallPkg(args[1], false, false, true);
                }
                else
                {
                    List<String> argss = new List<String>();
                    foreach (String arg in args)
                    {
                        argss.Add(arg);
                    }
                    argss.RemoveAt(0);
                    foreach (String arg in argss)
                    {
                        Console.WriteLine("================================================================================");
                        InstallPkg(arg, true, false, true);
                    }
                }
            }
        }
        else if (action == "listall")
        {
            ListPackages();
        }
        else if (action == "listinstalled")
        {
            ListPackages("installed");
        }
        else if (action == "cleanup")
        {
            CleanUp(true);
        }
        else if (action == "pathadd")
        {
            AddToPath();
        }
        else if (action == "remove")
        {
            if (args.Length == 2)
            {
                RemovePKG(args[1], output);
            }
            else
            {
                Console.WriteLine("Which package do you want to remove?");
                string packageName = Console.ReadLine();
                RemovePKG(packageName, output);            }
        }
        else if (action == "up" || action == "update") DataUpdate();
        else if (action == "upg" || action == "upgrade") CheckForAppUpdates(true, true, true);
        else if (action == "ak" || action == "appkit")
        {
            Console.WriteLine("Please write the appkit txt file path");
            string kitpath = @Console.ReadLine();
            if (kitpath != null) AppKits(kitpath);
            else Console.WriteLine("Kit path has to be something another (not null)");
        }
        else if (action == "spmup") VersionUpdate(curbranch);
        else if (action == "se" || action == "search")
        {
            if (!argav)
            {
                Console.WriteLine("Keyword: ");
                string Package = Console.ReadLine();
                if (Package != null)
                {
                    SearchPackages(Package, args);
                }
                else { Console.WriteLine("ERROR: Keyword can't be null"); PressAnyKey("exit", true); }
            }
            else
            {
                if (args.Length == 2)
                {
                    SearchPackages(args[1], args);
                }
            }
        }
        else if (action == "help" || action == "h")
        {
            if (output) Console.WriteLine("To get help just open the app without any options!");
        }
        else
        {
            if (output) Console.WriteLine("Launch the app without any options to get help!");
        }
        PressAnyKey("exit", true, 0, output);
    }
    public static void AddToPath(string newentry = @"C:\SPM")
    {
        string path = Environment.GetEnvironmentVariable("Path");
        Debug.WriteLine("Path is: " + path);
        if (!path.Contains(newentry))
        {
            path = path + @";" + newentry;
        }
        Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.Machine);
        Debug.WriteLine("New path is: " + path);
    }
    public static void VersionUpdate(string ubranch, bool IsSwitch = false)
    {
        // Update databases and load current versions
        DataUpdate(false);
        CheckForAppUpdates(false, true, false);
        DataLoad(InstallDir + "currentversions.txt", "currentversions");
        if (!currentappnames.Contains("spmupdatemanager"))
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine("Installing SPM Update Manager...");
            Console.WriteLine("================================================================================");
            InstallPkg("spmupdatemanager", false, false, false);
        }
        Process PackageStartInfo = new Process();
        PackageStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\spmupdatemanager\\SharpPackageManagerUpdateUtility.exe";
        if (IsSwitch) PackageStartInfo.StartInfo.Arguments = ubranch + " 1";
        else PackageStartInfo.StartInfo.Arguments = ubranch + " " + currentversion;
        PackageStartInfo.StartInfo.UseShellExecute = true;
        PackageStartInfo.Start();
        System.Environment.Exit(0);

    }

    public static void CleanUp(bool downloadcache, bool output = true)
    {
        if (downloadcache)
        {
            string[] files = Directory.GetFiles(@"C:\SPM\Downloads");
            foreach (string file in files)
            {
                // Clean up downloads
                System.IO.File.Delete(file);
                Debug.WriteLine("Deleted file " + file);
                if (output) Console.WriteLine("================================================================================");
                if (output) Console.WriteLine("Deleted file " + file);
                if (output) Console.WriteLine("================================================================================");
            }
        }
    }
    public static void ListPackages(string type = "all")
    {
        DataLoad(InstallDir + "currentversions.txt", "currentversions");
        DataUpdate();
        CheckForAppUpdates(false, true, false);
        switch (type)
        {
            case "all":
                foreach (string Package in appnames)
                {
                    Console.WriteLine("PKG: " + Package);
                    int appverindex = appnames.IndexOf(Package);
                    int appver = updateversions[appverindex];
                    Console.WriteLine(" Latest Version: " + appver);
                    if (currentappnames.Contains(Package))
                    {
                        int curverindex = currentappnames.IndexOf(Package);
                        int curver = currentappversions[curverindex];
                        Console.WriteLine(" Current Version: " + curver);
                    }
                    Console.WriteLine("================================================================================");
                }
                break;
            case "installed":
                foreach (string Package in currentappnames)
                {
                    Console.WriteLine("PKG: " + Package);
                    int curverindex = currentappnames.IndexOf(Package);
                    int curver = currentappversions[curverindex];
                    Console.WriteLine(" Current Version: " + curver);
                    Console.WriteLine("================================================================================");
                }
                break;
        }
    }
    public static void AppKits(string AppKitFile)
    {

        List<String> kitappnames = new List<String>();
        using (StreamReader file = new StreamReader(AppKitFile))
        {
            string[] temp;
            int i = 0;
            string ln2;
            Console.WriteLine("================================================================================");
            while ((ln2 = file.ReadLine()) != null)
            {
                temp = ln2.Split("/n");
                Console.WriteLine(temp[i]);
                kitappnames.Add(temp[i]);
            }
        }
        Console.WriteLine("================================================================================");
        Console.WriteLine("Installing Packages...");
        Console.WriteLine("================================================================================");
        int finappcount = 0;
        while (finappcount < kitappnames.Count)
        {
            InstallPkg(kitappnames[finappcount], true, false, true);
            finappcount++;
        }
        PressAnyKey("exit", true);
    }

    public static void SearchPackages(string keyword, string[] args)
    {
        CheckForAppUpdates(false, true, false);
        DataUpdate(false);
        string installedtext = string.Empty;
        foreach (string package in appnames)
        {
            if (package.Contains(keyword))
            {
                string curver = "Not Installed";
                DataLoad(InstallDir + "currentversions.txt", "currentversions");
                if (currentappnames.Contains(package))
                {
                    // Check inf installed and if the app is installed mark it as installed
                    installedtext = " (installed)";
                    int curverindex = currentappnames.IndexOf(package);
                    curver = currentappversions[curverindex].ToString();
                }
                int appverindex = updateappnames.IndexOf(package);
                int ver = updateversions[appverindex];
                Console.WriteLine("================================================================================");
                Console.WriteLine("PKG: " + package + installedtext + " \n LATEST VERSION: " + ver + "\n INSTALLED VERSION: " + curver + "\n\n");
                Console.WriteLine("================================================================================");
            }
        }
        PressAnyKey("exit", true);
    }

    public static void InstallPkg(string Package, bool Multi = false, bool upgrade = false, bool output = true, bool Download=true, bool localinstall=true)
    {
        if (appnames.Contains(Package) || upgrade) // If package doesn't exist don't even try to install it
        {
            if (!upgrade) DataLoad(InstallDir + "currentversions.txt", "currentversions");
            if (currentappnames.Contains(Package) && !upgrade)
            {
                if (output) Console.WriteLine("ERROR: This Package is already installed. If you want to install it again remove it from the currentversions.txt file.");
                PressAnyKey("exit", true, -1, output);
            }
            if (output) Console.WriteLine("================================================================================");
            if (output) Console.WriteLine("By installing any of the packages you agree to the license agreement of the package."); // Legal notice
            if (output) Console.WriteLine("================================================================================");
            if (!upgrade && AreModulesLoaded && localinstall)
            {
                foreach (string module in modules)
                {
                    if (System.IO.File.Exists(module + "\\preinstallationhooks.py"))
                    {
                        Process HookStartInfo = new Process();
                        HookStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\python310\\python.exe";
                        HookStartInfo.StartInfo.UseShellExecute = true;
                        if (output) Console.WriteLine("================================================================================");
                        if (output) Console.WriteLine("Running pre-installation hook...");
                        if (output) Console.WriteLine("================================================================================");
                        HookStartInfo.StartInfo.Arguments = module + "\\preinstallationhooks.py " + Package;
                        HookStartInfo.Start();
                        HookStartInfo.WaitForExit();
                    }
                }
            }
            if (System.IO.File.Exists(InstallPath + "Downloads\\" + Package + ".zip")) System.IO.File.Delete(InstallPath + "Downloads\\" + Package + ".exe");
            string pkgdir = "C:\\SPM\\Downloads\\" + Package + ".zip";
            int pkgnumber = appnames.IndexOf(Package);

            if (!Directory.Exists("C:\\SPM\\Downloads\\")) Directory.CreateDirectory("C:\\SPM\\Downloads\\");
            // Stop people from using v1.X.X packages
            if (appurls[pkgnumber].EndsWith(".exe"))
            {
                if (output) Console.WriteLine("================================================================================");
                if (output) Console.WriteLine("ERROR: You're downloading/installing a legacy package! \nSPM v2.X.X DOES NOT SUPPORT legacy v1.X.X packages. \nIf your 'bultek' repo is http://bpmr.bultek.com.ua, change it to http://repo.bultek.com.ua/spm !");
                if (output) Console.WriteLine("================================================================================");
                PressAnyKey("exit", true, 1, output);
            }
            if (Download)
            {
                if (output) Console.WriteLine("================================================================================");
                Console.WriteLine("Downloading the package...");
                if (output) Console.WriteLine("================================================================================");
                using (WebClient pkgdl = new WebClient())
                {
                    // Download the package
                    if (!appurls[pkgnumber].StartsWith("!MIRRORURL")) { 
                        pkgdl.DownloadFile(appurls[pkgnumber], pkgdir);
                    }
                    else
                    {
                        string posturl = appurls[pkgnumber].Replace("!MIRRORURL", "");
                        string repourl = string.Empty;
                        bool br = false;
                        foreach (string repo in reponames)
                        {
                            Dictionary<string, List<string>> repoapps = GetRepoApps(repo);
                            foreach (KeyValuePair<string, List<string>> keyValue in repoapps)
                            {
                                if (keyValue.Value.Contains(Package))
                                {
                                    // Get the repo url
                                    int repourlindex = reponames.IndexOf(repo);
                                    repourl = repourls[repourlindex];
                                    br = true;
                                }
                                if (br)
                                {
                                    break;
                                }
                            }
                            if (br)
                            {
                                break;
                            }
                        }
                        List<string> mirrors = repourl.Split('\n').ToList();
                        int mirrorcount = mirrors.Count;
                        // Random integer between 0 and mirrorcount
                        Random rnd = new Random();
                        int rndmirror = rnd.Next(0, mirrorcount);
                        // Download the current repo
                        while (string.IsNullOrEmpty(mirrors[rndmirror]))
                        {
                            Debug.WriteLine(rndmirror);
                            rndmirror = rnd.Next(0, mirrorcount);
                        }
                        string mr = mirrors[rndmirror].Replace("/apps.txt", posturl);
                        pkgdl.DownloadFile(mr, pkgdir);
                    }
                    // Param1 = Link of file
                    // Param2 = Path to save
                }
            }
            if (System.IO.Directory.Exists(@"C:\SPM-APPS\" + Package))
            {
                // If package was removed not correctly
                System.IO.Directory.Delete(@"C:\SPM-APPS\" + Package, true);
            }
            if (localinstall)
            {
                bool success = true;
                if (output) Console.WriteLine("================================================================================");
                Console.WriteLine("Extracting the package...");
                if (output) Console.WriteLine("================================================================================");
                ZipFile.ExtractToDirectory(pkgdir, @"C:\SPM-APPS\" + Package);
                DataLoad(@"C:\SPM-APPS\" + Package + @"\AppData.spmdata", "AppData");
                if (type[0] == "exe")
                {
                    foreach (string exe in exectuable)
                    {
                        // Run executables in .exe file type
                        Process HookStartInfo = new Process();
                        HookStartInfo.StartInfo.FileName = @"C:\SPM-APPS\" + Package + "\\" + exe;
                        HookStartInfo.StartInfo.UseShellExecute = true;
                        try
                        {
                            HookStartInfo.Start();
                            HookStartInfo.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while starting " + exe);
                            Debug.WriteLine(ex.Message);
                            if (output) Console.WriteLine("Is app installed correctly? (Y/n)");
                            string answer = "Yes";
                            if (output) answer = Console.ReadLine();
                            answer = answer.ToLower();
                            if (answer.ToLower().StartsWith("n"))
                            {
                                success = false;
                                PressAnyKey("exit", true, 1);
                            }
                        }
                        System.IO.Directory.Delete(@"C:\SPM-APPS\" + Package, true);
                    }
                }
                else Debug.WriteLine("There are no dependencies");
                bool isCfgInstalled = true;
                if (type[0] == "configfile")
                {
                    if (output) Console.WriteLine("================================================================================");
                    if (output) Console.WriteLine("Installing config files that have been provided by " + Package);
                    if (output) Console.WriteLine("================================================================================");
                    foreach (string exe in exectuable)
                    {
                        string targetfilename = InstallDir + Path.GetFileName(exe);
                        if (System.IO.File.Exists(targetfilename) && !upgrade)
                        {
                            if (output) Console.WriteLine("WARNING: File " + targetfilename + " exists, what do you want to do with it?");
                            string ans = "no";
                            if (output) Console.WriteLine("Overwrite file? (y/N)");
                            if (output) ans = Console.ReadLine();
                            if (ans.StartsWith("y") || upgrade)
                            {
                                System.IO.File.Delete(targetfilename);
                                System.IO.File.Copy(exe, targetfilename);
                            }
                            else
                            {
                                isCfgInstalled = false;
                            }
                        }
                        else if (!System.IO.File.Exists(targetfilename) && !upgrade)
                        {
                            System.IO.File.Copy(exe, targetfilename);
                        }
                        else if (upgrade)
                        {
                            System.IO.File.Delete(targetfilename);
                            System.IO.File.Copy(exe, targetfilename);
                        }
                    }
                }

                if (!upgrade && success)
                {
                    if (AreModulesLoaded)
                    {
                        foreach (string module in modules)
                        {
                            if (System.IO.File.Exists(module + "\\postinstallationhooks.py"))
                            {
                                // Run post-transaction hooks
                                Process HookStartInfo = new Process();
                                HookStartInfo.StartInfo.FileName = @"C:\\SPM-APPS\\python310\\python.exe";
                                HookStartInfo.StartInfo.UseShellExecute = true;
                                if (output) Console.WriteLine("================================================================================");
                                if (output) Console.WriteLine("Running post-installation hook...");
                                if (output) Console.WriteLine("================================================================================");
                                HookStartInfo.StartInfo.Arguments = module + "\\postinstallationhooks.py " + Package;
                                HookStartInfo.Start();
                                HookStartInfo.WaitForExit();
                            }
                        }
                    }
                    if (type[0] == "zip")
                    {
                        // Add the app to machine path and maybe create shortcuts
                        if (output) Console.WriteLine("To acsess the app you just installed search for binary in the C:\\SPM-APPS\\" + Package + " folder! \nAlso you can try to launch it using the terminal (It's added to your PATH)!");
                        AddToPath(@"C:\SPM-APPS\" + Package);
                        if (output && exectuable.Count > 0) Console.WriteLine("Do you want to create a start menu shortcut for the package (recommended for GUI apps) (y/N)? ");
                        string answer = "no";
                        if (output) answer = Console.ReadLine();
                        if (answer.ToLower().StartsWith('y') && exectuable.Count > 0 && !upgrade)
                        {
                            string icon = string.Empty;
                            if (System.IO.File.Exists(@"C:\SPM-APPS\" + Package + @"\icon.ico")) icon = @"C:\SPM-APPS\" + Package + @"\icon.ico";
                            if (!string.IsNullOrEmpty(icon))
                            {
                                CreateShortcut(exectuable[0], Package, icon);
                            }
                            else CreateShortcut(exectuable[0], Package);
                        }
                    }
                    if (isCfgInstalled)
                    {
                        if (!upgrade) CheckForAppUpdates(false, true, false);
                        // "Register" the package
                        int ver = updateappnames.IndexOf(Package);
                        int appverindex = updateversions[ver];
                        currentappnames.Add(Package);
                        currentappversions.Add(appverindex);
                        string wrdata = "\n" + Package + ", " + appverindex;
                        Debug.WriteLine(wrdata);
                        WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
                    }
                }
                // Clear cache
                if (exectuable.Count > 0) exectuable.Clear();
                if (shortcuts.Count > 0) shortcuts.Clear();
                if (type.Count > 0) type.Clear();
                if (Multi || upgrade) PressAnyKey("continue", false);
                else PressAnyKey("exit", true);
            }
        }
        else Console.WriteLine("Please specify the package correctly!");

    }

    public static void CreateShortcut(string executable, string package, string icon = @"C:\SPM\icon.ico")
    {
        // Create a shortcut to Start Menu
        string shortcutPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\" + package + ".lnk";
        WshShell shell = new WshShell();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
        shortcut.TargetPath = executable;
        shortcut.WorkingDirectory = @"C:\SPM-APPS\" + package;
        shortcut.IconLocation = icon;

        shortcut.Save();
        Debug.WriteLine("Shortcut created!");
        // Thanks Copilot
    }
    public static void UpgradePKG(string pkg, bool multiple, bool output)
    {
        // Upgrade packages as intended
        if (AreModulesLoaded)
        {
            foreach (string module in modules)
            {
                if (System.IO.File.Exists(module + "\\preupgradehooks.py"))
                {
                    Process HookStartInfo = new Process();
                    HookStartInfo.StartInfo.FileName = @"C:\\SPM-APPS\\python310\\python.exe";
                    HookStartInfo.StartInfo.UseShellExecute = true;
                    if (output) Console.WriteLine("================================================================================");
                    if (output) Console.WriteLine("Running pre-upgrade hook...");
                    if (output) Console.WriteLine("================================================================================");
                    HookStartInfo.StartInfo.Arguments = module + "\\preupgradehooks.py " + pkg;
                    HookStartInfo.Start();
                    HookStartInfo.WaitForExit();
                }
            }
        }
        int latestappverindex = updateappnames.IndexOf(pkg);
        int currentappverindex = currentappnames.IndexOf(pkg);
        int currentappver = currentappversions[currentappverindex];
        int latestappverversion = updateversions[latestappverindex];
        if (latestappverversion > currentappver)
        {
            InstallPkg(pkg, multiple, true, output);
            currentappversions[currentappverindex] = latestappverversion;
        }
        if (AreModulesLoaded)
        {
            foreach (string module in modules)
            {
                if (System.IO.File.Exists(module + "\\postupgradehooks.py"))
                {
                    Process HookStartInfo = new Process();
                    HookStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\python310\\python.exe";
                    HookStartInfo.StartInfo.UseShellExecute = true;
                    if (output) Console.WriteLine("================================================================================");
                    if (output) Console.WriteLine("Running post-upgrade hook...");
                    if (output) Console.WriteLine("================================================================================");
                    HookStartInfo.StartInfo.Arguments = module + "\\postupgradehooks.py " + pkg;
                    HookStartInfo.Start();
                    HookStartInfo.WaitForExit();
                }
            }
        }
    }
    public static void RemovePKG(string package, bool output)
    {
        if (currentappnames.Contains(package))
        {
            if (output) Console.WriteLine("================================================================================");
            DataLoad(InstallDir + "currentversions.txt", "currentversions");
            System.IO.File.WriteAllText(InstallDir + "currentversions.txt", string.Empty);
            WriteData(InstallDir + "currentversions.txt", "placeholder, 1", "AppendToFile");
            if (output) Console.WriteLine("Removing App Data...");
            if (output) Console.WriteLine("================================================================================");
            System.IO.Directory.Delete(@"C:\SPM-APPS\" + package, true);
            int currentappindex = currentappnames.IndexOf(package);
            currentappversions.RemoveAt(currentappindex);
            currentappnames.Remove(package);
            foreach (string pack in currentappnames)
            {
                // Write current versions to currentappversions.txt
                int writeappverindex = currentappnames.IndexOf(pack);
                int writeappver = currentappversions[writeappverindex];
                string wrdata = "\n" + pack + ", " + writeappver;
                WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
            }
            if (output) Console.WriteLine("================================================================================");
            Console.WriteLine("Removing From PATH...");
            string path = Environment.GetEnvironmentVariable("Path");
            if (path.Contains(@"SPM-APPS\" + package))
            {
                Debug.WriteLine("Found in PATH");
                string[] pathdirs;
                pathdirs = path.Split(';');
                foreach (string pathdir in pathdirs)
                {
                    if (pathdir.Contains(package))
                    {
                        path = path.Replace(";" + pathdir, string.Empty);
                    }
                }
                Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.Machine);
            }
            Console.WriteLine("Removing from Start menu...");
            if (System.IO.File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\" + package + ".lnk"))
            {
                System.IO.File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\" + package + ".lnk");
            }
            Console.WriteLine("If this is a config/mirrorlist file, you will have to remove from C:\\SPM\\config yourself!");
        }
    }
    public static void CheckForAppUpdates(bool autoUpdate = true, bool download = true, bool output = true)
    {
        // Clear updates cache
        if (download && !autoUpdate)
        {
            if (updateversions != null && updateappnames != null)
            {
                updateversions.Clear();
                updateappnames.Clear();
            }
            if (output) Console.WriteLine("Downloading the latest versions info");
            using (WebClient datadl = new WebClient())
            {
                int i = 0;
                while (i < reponames.Count)
                {
                    if (output) Console.WriteLine("==============================================");
                    if (output) Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
                    if (output) Console.WriteLine("==============================================");
                    // Download latest versions info
                    string currepopath = InstallDir + "versions" + reponames[i] + ".txt";
                    // set download url to current repourls
                    if (repourls[i].Contains("\n")){
                        List<string> mirrors = repourls[i].Split("\n").ToList();
                        // Select a random mirror
                        Random rnd = new Random();
                        int rndmirror = rnd.Next(0, mirrors.Count);
                        while (string.IsNullOrEmpty(mirrors[rndmirror]))
                        {
                            rndmirror = rnd.Next(0, mirrors.Count);
                        }
                        string mirror = mirrors[rndmirror].Replace("/apps.txt", "/versions.txt");
                        datadl.DownloadFile(mirror, currepopath);
                        Debug.WriteLine("Downloaded " + mirror);
                    }
                    else {
                        datadl.DownloadFile(repourls[i].Replace("/apps.txt", "/versions.txt"), currepopath);
                    }
                        // Load latest versions info
                    DataLoad(currepopath, "updates");
                    i++;
                }
            }
        }

        if (output)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Checking For Updates...");
            Console.WriteLine("==============================================");
        }
        if (autoUpdate)
        {
            CheckForAppUpdates(false, true, false);
            using (WebClient datadl = new WebClient())
            {
                int i = 0;
                while (i < reponames.Count)
                {
                    if (output)
                    {
                        Console.WriteLine("==============================================");
                        Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
                        Console.WriteLine("==============================================");
                    }
                    // Download latest versions info
                    string currepopath = InstallDir + "versions" + reponames[i] + ".txt";
                    // Load latest versions info
                    DataLoad(currepopath, "updates");
                    i++;
                }
            }
            if (output) Console.WriteLine("Loading Data...");
            DataLoad(InstallDir + "currentversions.txt", "currentversions");
            // Check if any packages are installed and if user has updates
            bool updates = false;
            List<String> updatecount = new List<String>();
            foreach (string app in currentappnames)
            {
                if (app != "placeholder")
                {
                    int appindex = updateappnames.IndexOf(app);
                    int currentappindex = currentappnames.IndexOf(app);
                    if (currentappversions[currentappindex] < updateversions[appindex])
                    {
                        updatecount.Add(app);
                        updates = true;
                        Debug.WriteLine("App added to updatecount! (" + app + ")");
                    }
                }
            }

            if (currentappversions.Count == 0)
            {
                Console.WriteLine("You don't have any packages!");
                PressAnyKey("exit", true, 0, output);
            }
            else if (!updates && updatecount.Count == 0)
            {
                Console.WriteLine("No Updates available!");
                PressAnyKey("exit", true, 0, output);
            }
            else
            {
                bool multiple = true;
                if (updatecount.Count == 1)
                {
                    multiple = false;
                }
                CheckForAppUpdates(false, true, false);
                // Clear currentversions.txt
                DataLoad(InstallDir + "currentversions.txt", "currentversions");
                System.IO.File.WriteAllText(InstallDir + "currentversions.txt", string.Empty);
                WriteData(InstallDir + "currentversions.txt", "placeholder, 1", "AppendToFile");
                foreach (string update in updatecount)
                {
                    UpgradePKG(update, multiple, output);
                }
                foreach (string pack in currentappnames)
                {
                    // Write current versions to currentappversions.tx
                    int writeappverindex = updateappnames.IndexOf(pack);
                    int writeappver = updateversions[writeappverindex];
                    string wrdata = "\n" + pack + ", " + writeappver;
                    WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
                }
                PressAnyKey("exit", true);
            }
        }

    }

    public static void PressAnyKey(string what = "exit", bool exit = false, int exitcode = 0, bool output = true)
    {
        if (output)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("Press Any key to " + what + "...");
            Console.ReadKey();
            // exit the app

            if (exit)
            {
                System.Environment.Exit(exitcode);
            }
        }
    }
    public static void DataUpdate(bool Out = true)
    {
        appnames.Clear();
        appurls.Clear();
        using (WebClient srcdl = new WebClient())
        {
            int i = 0;
            do
            {
                string currepopath = InstallDir + "apps" + reponames[i] + ".txt";
                if (System.IO.File.Exists(currepopath)) System.IO.File.Delete(currepopath);

                if (Out == true)
                {
                    Console.WriteLine("==================================");
                    Console.WriteLine("Updating " + reponames[i]);
                    Console.WriteLine("==================================");
                }
                if (repourls[i].Contains("\n"))
                {
                    
                    List<string> mirrors = repourls[i].Split('\n').ToList();
                    int mirrorcount = mirrors.Count;
                    // Random integer between 0 and mirrorcount
                    Random rnd = new Random();
                    int rndmirror = rnd.Next(0, mirrorcount);
                    // Download the current repo
                    while (string.IsNullOrEmpty(mirrors[rndmirror]))
                    {
                        Debug.WriteLine(rndmirror);
                        rndmirror = rnd.Next(0, mirrorcount);
                    } 
                    string mr = mirrors[rndmirror];
                    srcdl.DownloadFile(mr, currepopath);
                }
                else
                {
                    srcdl.DownloadFile(repourls[i], currepopath);
                }

                i++;
                DataLoad(currepopath, "apps");

                // Param1 = Link of file
                // Param2 = Path to save
            } while (i != repourls.Count());

        }
    }
    public static Dictionary<string, List<string>> GetRepoApps(string repo)
    {
        string currepopath = InstallDir + "apps" + repo + ".txt";
        List<String> apps = new List<string>();
            using (StreamReader file = new StreamReader(@"C:\SPM\config\apps" + repo+".txt"))
            {
                string ln2;
                string[] ln3;
                while ((ln2 = file.ReadLine()) != null)
                {
                    ln3 = ln2.Split(", ");
                    repos.Add(ln3[0], ln3[1]);
                }
                if (repos != null)
                {
                    foreach (KeyValuePair<string, string> keyValue in repos)
                    {
                        apps.Add(keyValue.Key);
                    }
                    repos.Clear();
                    file.Close();
                }
            }
        Dictionary<string, List<string>> r = new Dictionary<string, List<string>>();
        r.Add(repo, apps);
        return r;
    }
    public static void WriteData(string File, string data, string Type)
    {
        if (Type == "AppendToFile")
        {
#pragma warning disable CS0162 // Unreachable code detected
            for (int i = 1; i <= 3; ++i)
            {
                try
                {
                    System.IO.File.AppendAllText(File, data);
                    break; // When done we can break loop
                }
                catch (IOException) when (i <= 3)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    Thread.Sleep(1000);
                    Console.WriteLine("Error writing file");
                }
                break;
            }
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
    public static void DataLoad(string File, string Type, bool loadapps = false)
    {
        using (StreamReader file = new StreamReader(File))
        {
            string ln2;
            string[] ln3;
            while ((ln2 = file.ReadLine()) != null)
            {
                ln3 = ln2.Split(", ");
                repos.Add(ln3[0], ln3[1]);
            }
            if (repos != null)
            {
                switch (Type)
                {
                    case "repos":
                        repourls.Clear();
                        reponames.Clear();
                        break;
                    case "currentversions":
                        currentappversions.Clear();
                        currentappnames.Clear();
                        break;

                }
                foreach (KeyValuePair<string, string> keyValue in repos)
                {
                    switch (Type)
                    {
                        // Add the apps to responding lists
                        case "apps":
                            if (keyValue.Key != "placeholder" && !appnames.Contains(keyValue.Key))
                            {
                                appurls.Add(keyValue.Value);
                                appnames.Add(keyValue.Key);
                            }
                            break;
                        case "repos":
                            if (keyValue.Value.StartsWith("!MIRRORLIST=")) {
                                string mirrorlistfile = keyValue.Value.Replace("!MIRRORLIST=", "");
                                // Read mirrorlist file
                                string mirrors = System.IO.File.ReadAllText(mirrorlistfile);
                                foreach (char c in mirrors)
                                {
                                    if (c==' ')
                                    {
                                        mirrors.Remove(c);
                                    }
                                }
                                repourls.Add(mirrors);
                            }
                            else
                            {
                                repourls.Add(keyValue.Value);
                            }
                            reponames.Add(keyValue.Key);
                            break;
                        case "updates":
                            if (keyValue.Key != "placeholder")
                            {
                                updateversions.Add(int.Parse(keyValue.Value));
                                updateappnames.Add(keyValue.Key);
                            }
                            break;
                        case "currentversions":
                            if (keyValue.Key != "placeholder")
                            {
                                currentappversions.Add(int.Parse(keyValue.Value));
                                currentappnames.Add(keyValue.Key);
                            }
                            break;
                        case "AppData":
                            if (keyValue.Key == "exe")
                            {
                                exectuable.Add(keyValue.Value);
                            }
                            else if (keyValue.Key == "type")
                            {
                                type.Add(keyValue.Value);
                            }
                            break;
                    }
                }
            }
            repos.Clear();
            file.Close();
        }
    }

    public static string[] modules;
}