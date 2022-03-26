// Some parts of this code were made by GitHub Copilot. If any of this code violates license of other project, most probably it's written by Github Copilot. If you have any questions or concerns contact GitHub and Microsoft.
// This Code is licensed under the BSD 2 Clause License.
// Copyright (c) 2022, BultekDev / SharpPackageManager
// All rights reserved.

using IWshRuntimeLibrary;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
#pragma warning disable
public class SharpPackageManager
{
    public static string StartMenuDirectory = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\SPM-APPS";
    public static bool AreModulesLoaded = false;
    public static int latestversion;
    public static int currentversion = 27;


    public static string appversion = "v2.3.0 - PTB 5";
    public static string curbranch = "ptb";

    public static int currentapiversion = 2;

    public static string? tag;
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();
    public static List<String> updateappnames = new List<String>();
    public static List<int> updateversions = new List<int>();
    public static List<String> currentappnames = new List<String>();
    public static List<int> currentappversions = new List<int>();
    public static string InstallDir = "C:\\SPM\\config\\";
    public static string InstallPath = "C:\\SPM\\";
    private static List<String> dependencies = new List<String>();
    private static List<String> exectuable = new List<String>();

    private static List<String> shortcuts = new List<String>();
    private static List<String> type = new List<String>();

    private static Dictionary<string, string> repos = new Dictionary<string, string>();

    public static void Main(string[] args)
    {
        MAIN(args);
    }
    public static void MAIN(string[] args, bool launchmainapp = true, bool output = true, bool forexit = false)
    {
        Debug.WriteLine(currentversion);
        if (output)
        {
            Console.Title = "SharpPackageManager";
            Debug.WriteLine(Console.LargestWindowWidth + "x" + Console.LargestWindowHeight);
        }


        if (System.IO.Directory.Exists("C:\\SPM\\futureversion") && !System.IO.File.Exists("C:\\SPM\\futureversion\\unlock.txt") && !System.IO.File.Exists(InstallDir + "clean.txt"))
        {
            // Start upgrade process
            if (output) Console.WriteLine("Unlocking update on app start and executing the app...");
            System.IO.File.Create("C:\\SPM\\futureversion\\unlock.txt");
            Process PackageStartInfo = new Process();
            PackageStartInfo.StartInfo.FileName = ("C:\\SPM\\futureversion\\SPM\\SharpPackageManager.exe");
            PackageStartInfo.StartInfo.UseShellExecute = true;
            PackageStartInfo.StartInfo.Verb = "runas";
            PackageStartInfo.Start();
            System.Environment.Exit(0);
        }
        else if (System.IO.File.Exists(@"C:\\SPM\\futureversion\\unlock.txt")) {
            // List all files in the update folder and copy them to the main folder
            if (output) Console.WriteLine("Updating files...");
            foreach (string file in System.IO.Directory.GetFiles(@"C:\\SPM\\futureversion"))
            {
                if (file != "currentversions.txt" && file != "sources.txt")
                {
                    System.IO.File.Copy(file, file.Replace(@"C:\\SPM\\futureversion\", @"C:\SPM"), true);
                }
                System.IO.File.Delete(@"C:\SPM\futureversion\unlock.txt");
                System.IO.File.Create(InstallDir + "clean.txt");
            }
        }
        else if (System.IO.File.Exists(InstallDir + "clean.txt"))
        {
            // Clean the update folder
            if (output) Console.WriteLine("Cleaning update cache folder...");
            System.IO.Directory.Delete(@"C:\SPM\futureversion", true);
            System.IO.File.Delete(InstallDir + "clean.txt");
        }
        else
        {
            MainApp(args, output);
        }

    }
    public static void MainApp(string[] args, bool output = true)
    {
        string action = "null";
        bool argav = false;
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
                PackageStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\python310\\python.exe";
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

        if (!System.IO.Directory.Exists("C:\\SPM\\Downloads")) System.IO.Directory.CreateDirectory("C:\\SPM\\Downloads");
        if (!System.IO.Directory.Exists("C:\\SPM\\config")) System.IO.Directory.CreateDirectory("C:\\SPM\\config");

        if (!System.IO.File.Exists(InstallDir + "currentversions.txt"))
        {
            Console.WriteLine("ERROR: Can't find currentversions.txt, please create it and set it up!");
            PressAnyKey("exit", true, 1);
        }
        //if (!System.IO.File.Exists(InstallDir + "latestversions.txt")) System.IO.File.Create(InstallDir + "latestversions.txt");

        DataLoad(InstallDir + "sources.txt", "repos");
        DataLoad(InstallDir + "currentversions.txt", "currentversions");
        if (System.IO.File.Exists(InstallDir + "latestversions.txt")) DataLoad(InstallDir + "latestversions.txt", "updates");

        //if (System.IO.File.Exists(InstallDir + "appsbultek.txt")) DataLoad(InstallDir + "appsbultek.txt", "apps");
        foreach (string repo in reponames)
        {
            if (System.IO.File.Exists(InstallDir + "apps" + repo + ".txt")) DataLoad(InstallDir + "apps" + repo + ".txt", "apps");
        }
        //DataUpdate(false);
        //DataLoad(InstallDir + "apps.txt", "apps");
        if (args.Length == 0 && output)
        {
            Console.WriteLine("Sharp Package Manager by Bultek. " + appversion);
            Console.WriteLine("Sharp Package Manager API compatibility Version: " + currentapiversion);
            Console.WriteLine("Note: We recommend updating database!");
            Console.WriteLine("Note: We don't recommend using long commands in interactive mode!");
            Console.WriteLine("Please choose your action! \n \n");
            Console.WriteLine("Install a package (Command: i, install)");
            Console.WriteLine("Install an AppKit (Command: ak, appkit)");
            Console.WriteLine("Update database (Command: up, update)");
            Console.WriteLine("Check for SPM updates (Command: spmup)");
            Console.WriteLine("Check for app updates and upgrade packages (Command: upg, upgrade)");
            Console.WriteLine("Search for packages (Command: se, search)");
            Console.WriteLine("Switch branch (this is kinda risky! Command: swbr, switchbranch)");
            Console.WriteLine("Remove a package (Works only with .zip type packages. Command: remove)");
            Console.WriteLine("Add SPM to path (Command: pathadd)");
            Console.WriteLine("Clean up (Command: cleanup)");
            Console.WriteLine("List Packages (listall/listinstalled)");
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
                Console.WriteLine("Package to install (note: you can install only one package)");
                string Package = Console.ReadLine();
                if (Package != null)
                {
                    InstallPkg(Package, false, false, true);
                }
                else { Console.WriteLine("ERROR: Package can't be null"); PressAnyKey("exit", true); }
            }
            else
            {
                if (args.Length == 2)
                {
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
                RemovePKG(args[1]);
            }
            else
            {
                Console.WriteLine("Which Package you want to remove?");
                string packageName = Console.ReadLine();
                RemovePKG(packageName);
            }
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
        else if (action == "swbr" || action == "switchbranch")
        {
            if (curbranch == "ptb")
            {
                SwitchBranch("master", args);
            }
            else SwitchBranch("ptb", args);
        }
        else if (action == "help" || action == "h")
        {
            Console.WriteLine("To get help just open the app without any options!");
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
    public static void SwitchBranch(string Branch, string[] args)
    {
        VersionUpdate(Branch, true);
    }
    public static void VersionUpdate(string branch, bool IsSwitch = false)
    {
        Console.WriteLine("Loading latest versions info...");
        if (System.IO.File.Exists("C:\\temp\\latestversioninfo.spmvi")) System.IO.File.Delete("C:\\temp\\latestversioninfo.spmvi");
        if (System.IO.File.Exists("C:\\temp\\latestversiontag.spmvi")) System.IO.File.Delete("C:\\temp\\latestversiontag.spmsvi");
        using (WebClient tagdl = new WebClient())
        {
            tagdl.DownloadFile("https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/raw/versioncontrol/" + branch + ".spmvi", "C:\\temp\\latestversioninfo.spmvi");
            tagdl.DownloadFile("https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/raw/versioncontrol/" + branch + "tag.spmvi", "C:\\temp\\latestversiontag.spmvi");
            // Param1 = Link of file
            // Param2 = Path to save
        }
        // Read latest version info
        using (StreamReader file = new StreamReader("C:\\temp\\latestversioninfo.spmvi"))
        {
            latestversion = int.Parse(file.ReadLine());
        }
        if (latestversion > currentversion || IsSwitch)
        {
            Console.WriteLine("Downloading update...");
            using (WebClient tagdl = new WebClient())
            {
                //Console.WriteLine("Downloading versions info...");
                tagdl.DownloadFile("http://repo.bultek.com.ua/SPM-BINARY/SPM-" + branch + ".zip", "C:\\SPM.zip");
                // Param1 = Link of file
                // Param2 = Path to save
            }
            Console.WriteLine("Pre-Configuring update...");
            if (System.IO.Directory.Exists("C:\\SPM\\futureversion")) System.IO.Directory.Delete("C:\\SPM\\futureversion", true);
            if (!System.IO.Directory.Exists("C:\\SPM\\futureversion"))
            {
                System.IO.Directory.CreateDirectory("C:\\SPM\\futureversion");
                ZipFile.ExtractToDirectory("C:\\SPM.zip", "C:\\SPM\\futureversion");
            }
            Console.WriteLine("Please, restart the app to continue!");
            PressAnyKey("exit", true, 0, true);
        }
        else Console.WriteLine("You have the latest version");
        PressAnyKey("exit", true);
    }

    public static void CleanUp(bool downloadcache, bool output = true)
    {
        if (downloadcache)
        {
            string[] files = Directory.GetFiles(@"C:\SPM\Downloads");
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
                Debug.WriteLine("Deleted file " + file);
                if (output) Console.WriteLine("Deleted file " + file);
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
                }
                break;
            case "installed":
                foreach (string Package in currentappnames)
                {
                    Console.WriteLine("PKG: " + Package);
                    int curverindex = currentappnames.IndexOf(Package);
                    int curver = currentappversions[curverindex];
                    Console.WriteLine(" Current Version: " + curver);
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
            Console.WriteLine("You will install: ");
            while ((ln2 = file.ReadLine()) != null)
            {
                temp = ln2.Split("/n");
                Console.WriteLine(temp[i]);
                kitappnames.Add(temp[i]);
            }
        }
        Console.WriteLine("Installing Packages...");
        int finappcount = 0;
        while (finappcount < kitappnames.Count) ;
        {
            InstallPkg(kitappnames[finappcount], false, false, true);
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
                    installedtext = " (installed)";
                    int curverindex = currentappnames.IndexOf(package);
                    curver = currentappversions[curverindex].ToString();
                }
                int appverindex = updateappnames.IndexOf(package);
                int ver = updateversions[appverindex];

                Console.WriteLine("PKG: " + package + installedtext + " \n LATEST VERSION: " + ver + "\n INSTALLED VERSION: " + curver + "\n\n");
            }
        }
        PressAnyKey("exit", true);
    }
    public static void InstallPkg(string Package, bool Multi = false, bool upgrade = false, bool output = true)
    {
        if (appnames.Contains(Package) || upgrade)
        {
            if (!upgrade) CheckForAppUpdates(false, true, output);
            if (!upgrade) DataLoad(InstallDir + "currentversions.txt", "currentversions");
            if (currentappnames.Contains(Package) && !upgrade)
            {
                if (output) Console.WriteLine("ERROR: This Package is already installed. If you want to install it again remove it from the currentversions.txt file.");
                PressAnyKey("exit", true, -1, output);
            }
            if (!upgrade && AreModulesLoaded)
            {
                foreach (string module in modules)
                {
                    if (System.IO.File.Exists(module + "\\preinstallationhooks.py"))
                    {
                        Process HookStartInfo = new Process();
                        HookStartInfo.StartInfo.FileName = "C:\\SPM-APPS\\python310\\python.exe";
                        HookStartInfo.StartInfo.UseShellExecute = true;
                        Console.WriteLine("Running pre-installation hook...");
                        HookStartInfo.StartInfo.Arguments = module + "\\preinstallationhooks.py " + Package;
                        HookStartInfo.Start();
                        HookStartInfo.WaitForExit();
                    }
                }
            }
            if (System.IO.File.Exists(InstallPath + "Downloads\\" + Package + ".zip")) System.IO.File.Delete(InstallPath + "Downloads\\" + Package + ".exe");
            string pkgdir = "C:\\SPM\\Downloads\\" + Package + ".zip";
            int pkgnumber = appnames.IndexOf(Package);
            //Console.WriteLine(pkgnumber);
            if (!Directory.Exists("C:\\SPM\\Downloads\\")) Directory.CreateDirectory("C:\\SPM\\Downloads\\");
            if (appurls[pkgnumber].EndsWith(".exe"))
            {
                Console.WriteLine("ERROR: You're downloading a legacy package! \nSPM v2.X.X DOES NOT SUPPORT legacy v1.X.X packages. \nIf your 'bultek' repo is http://bpmr.bultek.com.ua, change it to http://repo.bultek.com.ua/spm !");
                PressAnyKey("exit", true);
            }
            Console.WriteLine("Downloading the package...");
            using (WebClient pkgdl = new WebClient())
            {
                pkgdl.DownloadFile(appurls[pkgnumber], pkgdir);
                // Param1 = Link of file
                // Param2 = Path to save
            }
            if (System.IO.Directory.Exists(@"C:\SPM-APPS\" + Package))
            {
                System.IO.Directory.Delete(@"C:\SPM-APPS\" + Package, true);
                //System.IO.Directory.CreateDirectory(@"C:\SPM-\APPS\"+ Package);
            }
            /*else if (!System.IO.Directory.Exists(@"C:\SPM-APPS\"+ Package)) {
                System.IO.Directory.CreateDirectory(@"C:\SPM-\APPS\"+ Package);
            }*/
            if (Directory.Exists(@"C:\SPM-APPS\" + Package))
            {
                Directory.Delete(@"C:\SPM-APPS\" + Package);
            }
            bool execerrors;
            bool success = true;
            Console.WriteLine("Extracting the package...");
            ZipFile.ExtractToDirectory(pkgdir, @"C:\SPM-APPS\" + Package);
            DataLoad(@"C:\SPM-APPS\" + Package + @"\AppData.spmdata", "AppData");
            if (type[0] == "exe")
            {
                foreach (string exe in exectuable)
                {
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
                        Console.WriteLine("Is app installed correctly? (Y/n)");
                        string answer = "Yes";
                        answer = Console.ReadLine();
                        answer = answer.ToLower();
                        if (answer != "yes" || answer != "y")
                        {
                            success = false;
                            PressAnyKey("exit", true, 1);
                        }
                    }
                    System.IO.Directory.Delete(@"C:\SPM-APPS\" + Package, true);
                }
            }
            List<string> deps = new List<string>();
            deps = dependencies;
            if (deps.Count > 0 && success)
            {
                foreach (string dependency in dependencies)
                {
                    if (!currentappnames.Contains(dependency))
                    {
                        Debug.WriteLine("Installing dependency " + dependency);
                        InstallPkg(dependency, true, false, output);
                        Debug.WriteLine("Dependency " + dependency + "has been installed");
                    }
                }
            }
            else Debug.WriteLine("Dependencies are null");



            if (!upgrade && success)
            {
                int ver = updateappnames.IndexOf(Package);
                int appverindex = updateversions[ver];
                currentappnames.Add(Package);
                currentappversions.Add(appverindex);
                string wrdata = "\n" + Package + ", " + appverindex;
                Debug.WriteLine(wrdata);
                WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
                if (AreModulesLoaded)
                {
                    foreach (string module in modules)
                    {
                        if (System.IO.File.Exists(module + "\\postinstallationhooks.py"))
                        {
                            Process HookStartInfo = new Process();
                            HookStartInfo.StartInfo.FileName = @"C:\\SPM-APPS\\python310\\python.exe";
                            HookStartInfo.StartInfo.UseShellExecute = true;
                            Console.WriteLine("Running post-installation hook...");
                            HookStartInfo.StartInfo.Arguments = module + "\\postinstallationhooks.py " + Package;
                            HookStartInfo.Start();
                            HookStartInfo.WaitForExit();
                        }
                    }
                }
                if (!Directory.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\SPM-APPS"))
                {
                    Directory.CreateDirectory(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\SPM-APPS");
                }
                if (type[0] == "zip")
                {
                    if (output) Console.WriteLine("To acsess the app you just installed search for binary in the C:\\SPM-APPS\\" + Package + " folder! \nAlso you can try to launch it using the terminal (It's added to your PATH)!");
                    AddToPath(@"C:\SPM-APPS\" + Package);
                    if (output && exectuable.Count>0) Console.WriteLine("Do you want to create a start menu shortcut for the package (recommended for GUI apps) (y/N)? ");
                    string answer = "no";
                    if (output) answer = Console.ReadLine();
                    if (answer.ToLower().StartsWith('y') && exectuable.Count > 0)
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
            }
            if (exectuable.Count > 0) exectuable.Clear();
            if (shortcuts.Count > 0) shortcuts.Clear();
            if (dependencies.Count > 0) dependencies.Clear();
            if (type.Count > 0) type.Clear();
            if (Multi || upgrade) PressAnyKey("continue");
            else PressAnyKey("exit", true);
        }
        else Console.WriteLine("Please specify the package correctly!");

    }

    public static async Task CreateShortcut(string executable, string package, string icon = @"C:\SPM\icon.ico")
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
        if (AreModulesLoaded)
        {
            foreach (string module in modules)
            {
                if (System.IO.File.Exists(module + "\\preupgradehooks.py"))
                {
                    Process HookStartInfo = new Process();
                    HookStartInfo.StartInfo.FileName = @"C:\\SPM-APPS\\python310\\python.exe";
                    HookStartInfo.StartInfo.UseShellExecute = true;
                    if (output) Console.WriteLine("Running pre-upgrade hook...");
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
                    if (output) Console.WriteLine("Running post-upgrade hook...");
                    HookStartInfo.StartInfo.Arguments = module + "\\postupgradehooks.py " + pkg;
                    HookStartInfo.Start();
                    HookStartInfo.WaitForExit();
                }
            }
        }
    }
    public static void RemovePKG(string package)
    {
        if (currentappnames.Contains(package))
        {
            DataLoad(InstallDir + "currentversions.txt", "currentversions");
            System.IO.File.WriteAllText(InstallDir + "currentversions.txt", string.Empty);
            WriteData(InstallDir + "currentversions.txt", "placeholder, 1", "AppendToFile");
            Console.WriteLine("Removing App Data...");
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
                //Console.WriteLine("Trying to write version info...");
                WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
            }
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
                Console.WriteLine("Removing from Start menu...");
                if (System.IO.File.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\" + package + ".lnk"))
                {
                    System.IO.File.Delete(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\" + package + ".lnk");
                }
            }
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
                    if (output) Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
                    // Download latest versions info
                    string currepopath = InstallDir + "versions" + reponames[i] + ".txt";
                    // set download url to current repourls
                    datadl.DownloadFile(repourls[i] + "/versions.txt", currepopath);
                    // Load latest versions info
                    DataLoad(currepopath, "updates");
                    i++;
                }
            }
        }

        if (output) Console.WriteLine("Checking For Updates...");
        if (autoUpdate)
        {
            CheckForAppUpdates(false, true, false);
            using (WebClient datadl = new WebClient())
            {
                int i = 0;
                while (i < reponames.Count)
                {
                    if (output) Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
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
                    //Console.WriteLine("Trying to write version info...");
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
        //repos.Clear();
        using (WebClient srcdl = new WebClient())
        {
            int i = 0;
            do
            {

                string currepopath = InstallDir + "apps" + reponames[i] + ".txt";
                if (System.IO.File.Exists(currepopath)) System.IO.File.Delete(currepopath);

                //Console.WriteLine(repourls[i]);
                if (Out == true) Console.WriteLine("Updating " + reponames[i]);
                srcdl.DownloadFile(repourls[i] + "/apps.txt", currepopath);

                i++;
                DataLoad(currepopath, "apps");

                //Console.WriteLine(appnames[i]);
                // Param1 = Link of file
                // Param2 = Path to save
            } while (i != repourls.Count());
            i = 0;

        }
    }
    public static void WriteData(string File, string data, string Type)
    {
        if (Type == "AppendToFile")
        {
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
                        case "apps":
                            if (keyValue.Key != "placeholder" && !appnames.Contains(keyValue.Key))
                            {
                                appurls.Add(keyValue.Value);
                                appnames.Add(keyValue.Key);
                            }
                            break;
                        case "repos":
                            repourls.Add(keyValue.Value);
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
                            if (keyValue.Key == "dep")
                            {
                                dependencies.Add(keyValue.Value);
                            }
                            else if (keyValue.Key == "exe")
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
public static class Extensions
{
    public static string Filter(this string str, List<char> charsToRemove)
    {
        // return String.Join(String.Empty, str.Split(charsToRemove.ToArray()));

        return String.Concat(str.Split(charsToRemove.ToArray()));
    }
}