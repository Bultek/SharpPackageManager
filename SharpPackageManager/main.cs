
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;
public class SharpPackageManager
{
    public static int latestversion;
    public static int currentversion =  5;
    public static string curbranch = "ptb";
    public static string?    tag;
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
            PressAnyKey();
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
                System.IO.File.Copy(upfile, destFile, true);

            }
            System.IO.File.Create(InstallDir + "clean.txt");
            Console.WriteLine("Please restart the app!");
            PressAnyKey();
        }
        else if (System.IO.File.Exists("C:\\SPM\\config\\clean.txt"))
        {
            Console.WriteLine("Cleaning Up...");
            System.IO.Directory.Delete(InstallPath + "futureversion", true);
            System.IO.File.Delete("C:\\SPM.zip");
            System.IO.File.Delete(InstallDir + "clean.txt");
            Console.WriteLine("Update Finished!");
            PressAnyKey();
        }
        else MainApp();
       
    }
    public static void MainApp()
    {

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
        Console.WriteLine("Please choose your action! And Before installing something update database please \n \n");
        Console.WriteLine("Install a package (Command: i) \n \n");
        Console.WriteLine("Install an AppKit (Command: ak) \n \n");
        Console.WriteLine("Update database (Command: up) \n \n");
        Console.WriteLine("Check for SPM updates (Command: spmup)");
        string action = Console.ReadLine();
        if (action == "i") // | action == "install")
        {
            SmartPkgInstall();
        }
        else if (action == "up") DataUpdate();
        else if (action == "upg") CheckForAppUpdates();
        else if (action == "upgo") CheckForAppUpdates(true, false);
        else if (action == "upd") CheckForAppUpdates(false, true); 
        else if (action == "ak")
        {
            Console.WriteLine("Please write the appkit txt file path (also please enter path with double \\ (example C:\\\\example\\\\appkit.txt)");
            string kitpath = (Console.ReadLine());
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
            Console.WriteLine("You wil install: ");
            while ((ln2 = file.ReadLine()) != null)
            {
                temp = ln2.Split("/n");
                Console.WriteLine(temp[i]);
                kitappnames.Add(temp[i]);
            }
        }
        Console.WriteLine("Installing Packages...");
        int finappcount = 0;
        do
        {
            InstallPkg(kitappnames[finappcount], true);
            finappcount++;
        }while (finappcount < kitappnames.Count);
        PressAnyKey();
    }
    public static void InstallPkg(string Package, bool Multi=false)
    {
        if (appnames.Contains(Package))
        {
            CheckForAppUpdates(false, true);
            //CheckForAppUpdates(false);
            //if (System.IO.File.Exists(InstallDir + "currentversions.txt")) System.IO.File.Delete(InstallDir + "currentversions.txt");
            DataUpdate();
            DataLoad(InstallDir + "currentversions.txt", "currentversions");
            //DataLoad(InstallDir + reponames[1], "a");
            currentappnames.Add(Package);
            currentappversions.Add(pkgver);
            int ver = updateappnames.IndexOf(Package);
            int appverindex = updateversions[ver];
            string wrdata = Package + ", " + appverindex;
            WriteData(InstallDir + "currentversions.txt", wrdata);
            if (Multi) PressAnyKey("continue");
            else PressAnyKey("exit", true);
        }
        else Console.WriteLine("Please specify the package correctly!");
    }
    public static void CheckForAppUpdates(bool autoUpdate=true, bool download=true)
    {
        // Clear updates cache
        if (download) {
            if (updateversions!=null && updateappnames!=null) {
                updateversions.Clear();
                updateappnames.Clear();
            }
            // Download the latest versions info 
            using (WebClient datadl = new WebClient()) {
                int i = 0;
                while (i < reponames.Count) {
                    Console.WriteLine("Updating " + reponames[i]); // Show which repo is being updated now.
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
            if (autoUpdate) {
                // Check if any packages are installed and if user has updates
                if (currentappversions.Count == 0) Console.WriteLine("You don't have any packages!");
                else if (updateappnames.Count == 0) Console.WriteLine("No Updates available!"); 
                else {
                    int a = 0;
                    int b = appnames.Count;
                    int x;
                    while (a < b) {
                        x = currentappnames.IndexOf(updateappnames[a]);
                        if (currentappversions[a]<updateversions[x]) {
                            DataLoad(InstallDir + "currentversions.txt", "currentversions");
                            // Update needed app
                            Console.WriteLine("Updating " + currentappnames[a]);
                            InstallPkg(currentappnames[a], true);
                            // Reload currentappversions
                            DataLoad(InstallDir+"currentversions.txt", "currentversions");
                            a++;
                        }
                    }
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
            MainApp();
        }
    public static void SmartPkgInstall()
    {
        Console.WriteLine("Which Package do you want to install?");
        string Package = Console.ReadLine();
        //char curchar;
        char space = ' ';
        int pkgspacecounter = 0;
        if (Package.Contains(' '))
        {
            string[] pkgs;
            //Console.WriteLine("Pered Furichem");
            foreach (char curchar in Package)
            {
                if (space == curchar)
                {
                    pkgspacecounter++;
                    //Console.WriteLine("+ pkgspacecounter");
                }
            }

            pkgs = Package.Split(' ');
            //Console.WriteLine("Pkgs splitted");
            if (pkgspacecounter > 0)
            {
                int i = 0;
                while (i < pkgspacecounter++)
                {
                    InstallPkg(pkgs[i], true);
                    i++;
                }
            }
        }
        else InstallPkg(Package);
    }
    public static void WriteData(string File, string data, string Type)
    {
        switch (Type) {
            case "WriteFile":
                using (StreamWriter sw = new StreamWriter(File)) sw.Write(data); 
                break;
            case "AppendToFile":
                System.IO.File.AppendAllText(File, data);
                break;
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
                    case "updates":
                        updateappnames.Clear();
                        updateversions.Clear();
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
                            appurls.Add(keyValue.Value);
                            appnames.Add(keyValue.Key);
                            break;
                        case "repos":
                            repourls.Add(keyValue.Value);
                            reponames.Add(keyValue.Key);
                            break;
                        case "updates":
                            updateversions.Add(int.Parse(keyValue.Value));
                            updateappnames.Add(keyValue.Key);
                            break;
                        case "currentversions":
                            currentappversions.Add(int.Parse(keyValue.Value));
                            currentappnames.Add(keyValue.Key);
                            break;
                    }
                }
            }
            repos.Clear();
            file.Close();
        }
    }
}