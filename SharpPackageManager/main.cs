
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
#pragma warning disable

public class SharpPackageManager
{
    public static bool AreModulesLoaded = false;
    public static int latestversion;
    public static string appversion = "v1.1 rc-1";
    public static int currentversion =  10;
    public static string curbranch = "master";
    public static string? tag;
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<char> spacecharacters = new List<char>();
    public static List<String> appurls = new List<String>();
    public static List<String> updateappnames = new List<String>();
    public static List<int> updateversions = new List<int>();
    public static List<String> currentappnames = new List<String>();
    public static List<int> currentappversions = new List<int>();
    public static string InstallDir = "C:\\SPM\\config\\";
    public static string InstallPath = "C:\\SPM\\";

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
   public static void Main(string[] args)
    {

        if (System.IO.Directory.Exists("C:\\SPM\\futureversion") && !System.IO.File.Exists("C:\\SPM\\futureversion\\unlock.txt") && !System.IO.File.Exists(InstallDir + "clean.txt"))
        {
            Console.WriteLine("Unlocking update on app start and executing the app...");
            System.IO.File.Create("C:\\SPM\\futureversion\\unlock.txt");
            Process PackageStartInfo = new Process();
            PackageStartInfo.StartInfo.FileName = ("C:\\SPM\\futureversion\\SPM\\SharpPackageManager.exe");
            PackageStartInfo.StartInfo.UseShellExecute = true;
            PackageStartInfo.StartInfo.Verb = "runas";
            PackageStartInfo.Start();
        }
        else if (System.IO.File.Exists("C:\\SPM\\futureversion\\unlock.txt") && System.IO.Directory.Exists("C:\\SPM\\futureversion") && !System.IO.Directory.Exists("C:\\SPM\\clean.txt"))
        {
            
            Console.WriteLine("Update is unlocked, starting the main upgrade script...");
            System.IO.File.Delete("C:\\SPM\\futureversion\\unlock.txt");
            Console.WriteLine("Copying Files...");
            string[] upfiles = System.IO.Directory.GetFiles("C:\\SPM\\futureversion\\SPM");
            foreach (string upfile in upfiles)
            {
                string fileName = System.IO.Path.GetFileName(upfile);
                string destFile = System.IO.Path.Combine(InstallPath + fileName);
                if (!upfile.Contains("currentversions") || !upfile.Contains("sources")) {
                    System.IO.File.Copy(upfile, destFile, true);
                }

            }
            System.IO.File.Create(InstallDir + "clean.txt");
            Console.WriteLine("Please restart the app!");
        }
        else if (System.IO.File.Exists("C:\\SPM\\config\\clean.txt"))
        {
            Console.WriteLine("Cleaning Up...");
            System.IO.Directory.Delete(InstallPath + "futureversion", true);
            System.IO.File.Delete("C:\\SPM.zip");
            System.IO.File.Delete(InstallDir + "clean.txt");
            Console.WriteLine("Update Finished!");
        }
        else
        {
            MainApp();
        }
       
    }
    public static void MainApp()
    {
        Debug.WriteLine("Loading Modules");
        if (File.Exists(@"C:\SPM\libspm.py") && Directory.Exists(@"C:\SPM\pythonlibspmruntime") && Directory.Exists(@"C:\SPM\modules") && !AreModulesLoaded) {
            modules = Directory.GetDirectories(@"C:\SPM\modules");
            foreach (string module in modules) {
            if (File.Exists(module+"\\libspm.py")) {
                File.Delete(module+"\\libspm.py");
            }
            System.IO.File.Copy("C:\\SPM\\libspm.py", module+"\\libspm.py");
            Process PackageStartInfo = new Process();
            PackageStartInfo.StartInfo.FileName = @"C:\\SPM\\pythonlibspmruntime\\python.exe";
            PackageStartInfo.StartInfo.Arguments = module+"\\init.py";
            PackageStartInfo.StartInfo.UseShellExecute = true;
            PackageStartInfo.Start();
            PackageStartInfo.WaitForExit();
            Debug.WriteLine(module+" Should be loaded");
            Console.Clear();
            AreModulesLoaded=true;
            }
        }
        else Debug.WriteLine("No module loaded");

        if (!System.IO.Directory.Exists("C:\\SPM\\Downloads")) System.IO.Directory.CreateDirectory("C:\\SPM\\Downloads");
        if (!System.IO.Directory.Exists("C:\\SPM\\config")) System.IO.Directory.CreateDirectory("C:\\SPM\\config");
        if (!System.IO.File.Exists(InstallDir + "currentversions.txt")) System.IO.File.Create(InstallDir+"currentversions.txt");
        //if (!System.IO.File.Exists(InstallDir + "latestversions.txt")) System.IO.File.Create(InstallDir + "latestversions.txt");

        DataLoad(InstallDir + "sources.txt", "repos");
        DataLoad(InstallDir + "currentversions.txt", "currentversions");
        if(System.IO.File.Exists(InstallDir+"latestversions.txt")) DataLoad(InstallDir + "latestversions.txt", "updates");

        if (System.IO.File.Exists(InstallDir + "appsbultek.txt")) DataLoad(InstallDir + "appsbultek.txt", "apps");
        //DataUpdate(false);
        //DataLoad(InstallDir + "apps.txt", "apps");
        Console.WriteLine("Sharp Package Manager by Bultek. "+appversion);
        Console.WriteLine("Please choose your action! And Before installing something update database please \n \n");
        Console.WriteLine("Install a package (Command: i) \n \n");
        Console.WriteLine("Install an AppKit (Command: ak) \n \n");
        Console.WriteLine("Update database (Command: up) \n \n");
        Console.WriteLine("Check for SPM updates (Command: spmup) \n \n");
        Console.WriteLine("Check for app updates and upgrade packages (Command: upg)");
        string action = Console.ReadLine();
        if (action == "i")
        {
            Console.WriteLine("Package to install (note: you can install only one package)");
            string Package=Console.ReadLine();
            if (Package != null) {
                InstallPkg(Package);
            }
            else { Console.WriteLine("ERROR: Package can't be null"); PressAnyKey("exit", true); }
        }
        else if (action == "up") DataUpdate();
        else if (action == "upg") CheckForAppUpdates(true, true);
        else if (action == "ak")
        {
            Console.WriteLine("Please write the appkit txt file path");
            string kitpath = @Console.ReadLine();
            if (kitpath != null) AppKits(kitpath);
            else Console.WriteLine("Kit path has to be something another (not null)");
        }
        else if (action == "spmup") VersionUpdate(curbranch);
    }
        public static void VersionUpdate(string branch)
        {
            Console.WriteLine("Loading latest versions info...");
            if (File.Exists("C:\\temp\\latestversioninfo.bpmsvi")) File.Delete("C:\\temp\\latestversioninfo.bpmsvi");
            if (File.Exists("C:\\temp\\latestversiontag.bpmsvi")) File.Delete("C:\\temp\\latestversiontag.bpmsvi");
            using (WebClient tagdl = new WebClient())
            {
                tagdl.DownloadFile("https://github.com/Bultek/SharpPackageManager/raw/versioncontrol/" + branch + ".spmvi", "C:\\temp\\latestversioninfo.bpmsvi");
                tagdl.DownloadFile("https://github.com/Bultek/SharpPackageManager/raw/versioncontrol/" + branch + "tag.spmvi", "C:\\temp\\latestversiontag.bpmsvi");
                // Param1 = Link of file
                // Param2 = Path to save
            }
            using (StreamReader file = new StreamReader("C:\\temp\\latestversioninfo.bpmsvi"))
            {
                latestversion = int.Parse(file.ReadLine());
            }
            using (StreamReader file = new StreamReader("C:\\temp\\latestversiontag.bpmsvi"))
            {
                tag = file.ReadLine();
            }
            if (latestversion > currentversion)
            {
                Console.WriteLine("Downloading update...");
                using (WebClient tagdl = new WebClient())
                {
                    Console.WriteLine("Downloading versions info...");
                    tagdl.DownloadFile("https://github.com/Bultek/SharpPackageManager/releases/download/" + tag + "/SPM.zip", "C:\\SPM.zip");
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
        }
        else Console.WriteLine("You have the latest version");
        MainApp();
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
        while (finappcount < kitappnames.Count);
        {
            InstallPkg(kitappnames[finappcount], true);
            finappcount++;
        }
        PressAnyKey();
    }
    public static void InstallPkg(string Package, bool Multi=false, bool upgrade=false)
    {
        if (appnames.Contains(Package) || upgrade)
        {
            if (!upgrade) CheckForAppUpdates(false, true);
            if (!upgrade) DataLoad(InstallDir + "currentversions.txt", "currentversions");
            if (currentappnames.Contains(Package) && !upgrade) {
                Console.WriteLine("This Package is already installed. If you want to install it again remove it from the currentversions.txt file. \n WARNING: It may break something!");
            }
            if (!upgrade && AreModulesLoaded) {
                foreach (string module in modules) {
                if (File.Exists(module+"\\preinstallationhooks.py")){
                    Process HookStartInfo = new Process();
                    HookStartInfo.StartInfo.FileName = @"C:\\SPM\\pythonlibspmruntime\\python.exe";
                    HookStartInfo.StartInfo.UseShellExecute = true;
                    Console.WriteLine("Running pre-installation hook...");
                    HookStartInfo.StartInfo.Arguments = module+"\\preinstallationhooks.py "+Package;
                    HookStartInfo.Start();
                    HookStartInfo.WaitForExit();
                }
            }
        }
            if (System.IO.File.Exists(InstallPath + "Downloads\\" + Package + ".exe")) System.IO.File.Delete(InstallPath + "Downloads\\" + Package + ".exe");
            string pkgdir = "C:\\SPM\\Downloads\\" + Package + ".exe";
            int pkgnumber = appnames.IndexOf(Package);
            //Console.WriteLine(pkgnumber);
            if (!Directory.Exists("C:\\SPM\\Downloads\\")) Directory.CreateDirectory("C:\\SPM\\Downloads\\");
            Console.WriteLine("Downloading the package...");
            using (WebClient pkgdl = new WebClient())
            {
                pkgdl.DownloadFile(appurls[pkgnumber], pkgdir);
                // Param1 = Link of file
                // Param2 = Path to save
            }
            Process PackageStartInfo = new Process();
            PackageStartInfo.StartInfo.FileName = pkgdir;
            PackageStartInfo.StartInfo.UseShellExecute = true;
            PackageStartInfo.StartInfo.Verb = "runas";
            PackageStartInfo.Start();
            PackageStartInfo.WaitForExit();
            //CheckForAppUpdates(false);
            //if (System.IO.File.Exists(InstallDir + "currentversions.txt")) System.IO.File.Delete(InstallDir + "currentversions.txt");
            DataUpdate(false);
            //DataLoad(InstallDir + reponames[1], "a");


            if (!upgrade)
            {
                int ver = updateappnames.IndexOf(Package);
                int appverindex = updateversions[ver];
                currentappnames.Add(Package);
                currentappversions.Add(appverindex);
                string wrdata = "\n" + Package + ", " + appverindex;
                Console.WriteLine(wrdata);
                WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
                if (AreModulesLoaded) {
                foreach (string module in modules) {
                    if (File.Exists(module+"\\postinstallationhooks.py")){
                        Process HookStartInfo = new Process();  
                        HookStartInfo.StartInfo.FileName = @"C:\\SPM\\pythonlibspmruntime\\python.exe";
                        HookStartInfo.StartInfo.UseShellExecute = true;
                        Console.WriteLine("Running post-installation hook...");
                        HookStartInfo.StartInfo.Arguments = module+"\\postinstallationhooks.py "+Package;
                        HookStartInfo.Start();
                        HookStartInfo.WaitForExit();
                    }
                }
                }
            }
            if (Multi || upgrade) PressAnyKey("continue");
            else PressAnyKey("exit", true);
        }
        else Console.WriteLine("Please specify the package correctly!");
        
    }
    public static void UpgradePKG(string pkg, bool multiple) {
        if (AreModulesLoaded) {
        foreach (string module in modules) {
            if (File.Exists(module+"\\preupgradehooks.py")){
                Process HookStartInfo = new Process();  
                HookStartInfo.StartInfo.FileName = @"C:\\SPM\\pythonlibspmruntime\\python.exe";
                HookStartInfo.StartInfo.UseShellExecute = true;
                Console.WriteLine("Running pre-upgrade hook...");
                HookStartInfo.StartInfo.Arguments = module+"\\preupgradehooks.py "+pkg;
                HookStartInfo.Start();
                HookStartInfo.WaitForExit();
                }
            }
        }
        int latestappverindex = updateappnames.IndexOf(pkg);
        int currentappverindex = currentappnames.IndexOf(pkg);
        int currentappver = currentappversions[currentappverindex];
        int latestappverversion = updateversions[latestappverindex];
        if (latestappverversion>currentappver){
            InstallPkg(pkg, multiple, true);
            currentappversions[currentappverindex]=latestappverversion;
        }
        if (AreModulesLoaded) {
        foreach (string module in modules) {
            if (File.Exists(module+"\\postupgradehooks.py")){
                Process HookStartInfo = new Process();  
                HookStartInfo.StartInfo.FileName = @"C:\\SPM\\pythonlibspmruntime\\python.exe";
                HookStartInfo.StartInfo.UseShellExecute = true;
                Console.WriteLine("Running post-upgrade hook...");
                HookStartInfo.StartInfo.Arguments = module+"\\postupgradehooks.py "+pkg;
                HookStartInfo.Start();
                HookStartInfo.WaitForExit();
                }
            }
        }
    }
    public static void CheckForAppUpdates(bool autoUpdate=true, bool download=true, bool output=true)
    {
        // Clear updates cache
        if (download && !autoUpdate) {
            if (updateversions!=null && updateappnames!=null) {
                updateversions.Clear();
                updateappnames.Clear();
            }
            Console.WriteLine("Downloading the latest versions info");
            using (WebClient datadl = new WebClient()) {
                int i = 0;
                while (i < reponames.Count) {
                    if (output) Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
                    // Download latest versions info
                    string currepopath = InstallDir + "versions" + reponames[i] + ".txt";
                    // set download url to current repourls
                    datadl.DownloadFile(repourls[i]+"/versions.txt", currepopath);
                    // Load latest versions info
                    DataLoad(currepopath, "updates");
                    i++;
                }
            }
        }
            
            Console.WriteLine("Checking For Updates...");
            if (autoUpdate) {
            CheckForAppUpdates(false, true, false);
            using (WebClient datadl = new WebClient())
            {
                int i = 0;
                while (i < reponames.Count)
                {
                    Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
                    // Download latest versions info
                    string currepopath = InstallDir + "versions" + reponames[i] + ".txt";
                    // Load latest versions info
                    DataLoad(currepopath, "updates");
                    i++;
                }
            }
            Console.WriteLine("Loading Data");
            DataLoad(InstallDir + "currentversions.txt", "currentversions");
            // Check if any packages are installed and if user has updates
            bool updates = false;
                List<String> updatecount = new List<String>();
                foreach (string app in currentappnames)
                {
                    if (app!="placeholder"){
                        int appindex = updateappnames.IndexOf(app);
                        int currentappindex = currentappnames.IndexOf(app);
                        if (currentappversions[currentappindex]<updateversions[appindex]) {
                            updatecount.Add(app);
                            updates = true;
                            Debug.WriteLine("App added to updatecount! ("+app+")");
                        }
                    }
                }
                
                if (currentappversions.Count == 0) {
                    Console.WriteLine("You don't have any packages!");
                    PressAnyKey();
                }
                else if (!updates && updatecount.Count == 0) {
                    Console.WriteLine("No Updates available!"); 
                    PressAnyKey();
                }
                else {
                    bool multiple = true;
                    if (updatecount.Count==1) {
                        multiple=false;
                    }  
                            CheckForAppUpdates(false, true, false);
                            // Clear currentversions.txt
                            DataLoad(InstallDir + "currentversions.txt", "currentversions");
                            System.IO.File.WriteAllText(InstallDir+"currentversions.txt", string.Empty);
                            WriteData(InstallDir+"currentversions.txt", "placeholder, 1", "AppendToFile");
/*                            int finappcount = 0;
                            int ver = 0;
                            int appverindex = 0;
                            int apppos = 0;
                            foreach (string update in updatecount) {
                                    string app = updatecount[finappcount];
                                    InstallPkg(app, multiple, true);
                                    ver = updateappnames.IndexOf(app);
                                    appverindex = updateversions[ver];
                                    apppos = currentappnames.IndexOf(app);
                                    currentappversions[apppos] = appverindex;
                                    ver = 0;
                                    appverindex = 0;
                                    apppos = 0;
                            } 
*/
                            foreach (string update in updatecount){
                                UpgradePKG(update, multiple);
                            }
                            foreach (string pack in currentappnames)
                            {
                                // Write current versions to currentappversions.txt

                                int writeappverindex=updateappnames.IndexOf(pack);
                                int writeappver=updateversions[writeappverindex];
                                string wrdata = "\n" + pack + ", " + writeappver;
                                //Console.WriteLine("Trying to write version info...");
                                WriteData(InstallDir + "currentversions.txt", wrdata, "AppendToFile");
                            }
                            PressAnyKey();
                }
            }

    }
    
    public static void PressAnyKey(string what="exit", bool exit=false)
    {
        Console.WriteLine("Press Any key to "+what+"...");
        Console.ReadKey();
        // exit the app
        if (exit) System.Environment.Exit(0);
    }
        public static void DataUpdate(bool Out=true)
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
            if (Out) MainApp();
        }
    public static void WriteData(string File, string data, string Type)
    {
            if (Type == "AppendToFile") {
                for (int i=1; i <= 3; ++i) {
                    try {
                        System.IO.File.AppendAllText(File, data);
                        break; // When done we can break loop
                    }
                    catch (IOException e) when (i <= 3) {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    Thread.Sleep(1000);
                    Console.WriteLine("Error writing file");
                    }
                    break;
                }
            }       
    }
    public static void DataLoad(string File, string Type)
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
            if (repos!=null) {
                switch (Type) {
                    case "apps":
                        appurls.Clear();
                        appnames.Clear();
                        break;
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
                            if (keyValue.Key!="placeholder"){
                            appurls.Add(keyValue.Value);
                            appnames.Add(keyValue.Key);
                            }
                            break;
                        case "repos":
                            repourls.Add(keyValue.Value);
                            reponames.Add(keyValue.Key);
                            break;
                        case "updates":
                            if (keyValue.Key!="placeholder"){
                            updateversions.Add(int.Parse(keyValue.Value));
                            updateappnames.Add(keyValue.Key);
                            }
                            break;
                        case "currentversions":
                            if (keyValue.Key!="placeholder") {
                            currentappversions.Add(int.Parse(keyValue.Value));
                            currentappnames.Add(keyValue.Key);
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