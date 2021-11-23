
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;
public class SharpPackageManager
{
    public static int latestversion;
    public static int currentversion = 2;
    public static string curbranch = "ptb";
    public static string tag;
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();
    public static string InstallDir = "C:\\SPM\\config\\";
    public static string InstallPath = "C:\\SPM\\";

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        if (!System.IO.Directory.Exists("C:\\SPM\\futureversion") & System.IO.File.Exists("C:\\SPM\\futureversion\\unlock.txt")) 
        {
            Console.WriteLine("Unlocking update on app start and executing the app...");
            System.IO.File.Create("C:\\SPM\\futureversion\\unlock.txt");
            Process PackageStartInfo = new Process();
            PackageStartInfo.StartInfo.FileName = ("C:\\SPM\\futureversion\\SPM\\SharpPackageManager.exe");
            PackageStartInfo.StartInfo.UseShellExecute = true;
            PackageStartInfo.StartInfo.Verb = "runas";
            PackageStartInfo.Start();
        }
        else if (System.IO.File.Exists("C:\\SPM\\futureversion\\unlock.txt"))
        {
            Console.WriteLine("Update is unlocked, starting the main upgrade script...");
            System.IO.File.Delete("C:\\SPM\\futureversion\\unlock.txt");
            Console.WriteLine("Copying Files...");
            string[] upfiles = System.IO.Directory.GetFiles("C:\\SPM\\futureversion\\SPM");
            foreach (string upfile in upfiles)
            {
                string fileName= System.IO.Path.GetFileName(upfile);
                string destFile = System.IO.Path.Combine(InstallPath + fileName);
                System.IO.File.Copy(upfile, destFile, true);

            }
            Console.WriteLine("Cleaning Up...");
            System.IO.Directory.Delete(InstallPath + "futureversion", true);
            System.IO.File.Delete("C:\\SPM.zip");
            Console.WriteLine("Update Finished!");

        }
        else
        {
            DataLoad(InstallDir + "sources.txt", "repos");
            DataLoad(InstallDir + "apps.txt", "apps");
            Console.WriteLine("Please choose your action! \n \n");
            Console.WriteLine("Install a package (Command: i) \n \n");
            Console.WriteLine("Install an AppKit (Command: ak) \n \n");
            Console.WriteLine("Update database (Command: up) \n \n");
            Console.WriteLine("Check for SPM updates (Command: spmup)");
            string action = Console.ReadLine();
            if (action == "i") // | action == "install")
            {
                Console.WriteLine("Which Package do you want to install?");
                //string Package = Console.ReadLine();
                InstallPkg(Console.ReadLine());
            }
            else if (action == "up")
            {
                DataUpdate(InstallDir + "apps.txt", "apps");
            } //| action == "update");
            else if (action == "ak")
            {
                Console.WriteLine("Please write the appkit txt file path (also please enter path with double \\ (example C:\\\\example\\\\appkit.txt)");
                string kitpath = (Console.ReadLine());
                if (kitpath != null) AppKits(kitpath);
                else Console.WriteLine("Kit path has to be something another (not null)");
            }
            else if (action == "spmup") VersionUpdate(curbranch);
        }
        //InstallPkg("steam");
    }
    public static void VersionUpdate(string branch)
    {
        Console.WriteLine("Loading latest versions info...");
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

        if (branch == "ptb" || latestversion > currentversion)
        {
            Console.WriteLine("Downloading update...");
            using (WebClient tagdl = new WebClient())
            {
                Console.WriteLine("Loading latest versions info...");
                tagdl.DownloadFile("https://github.com/Bultek/SharpPackageManager/releases/download/"+tag+"/SPM.zip", "C:\\SPM.zip");
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
            System.IO.File.Create("C:\\SPM\\futureversion\\unlock.txt");
            Console.WriteLine("Please, restart the app to continue!");
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
            InstallPkg(kitappnames[finappcount]);
            finappcount++;
        }while (finappcount < kitappnames.Count);
    }
    public static void InstallPkg(string Package)
    {
        //if (Package == null) Console.WriteLine("Please specify the package!");
        if (appnames.Contains(Package))
        {
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

        }
        else Console.WriteLine("Please specify the package correctly!");
    }
    public static void DataUpdate(string File, string Type)
    {

        {
            using (WebClient srcdl = new WebClient())
            {
                int i = 0;
                do
                {
                    
                    Console.WriteLine("Updating "+reponames[i]);
                    srcdl.DownloadFile(repourls[i] + "/apps.txt", InstallDir+"apps.txt");
                    i++;
                    // Param1 = Link of file
                    // Param2 = Path to save
                } while (i != repourls.Count());
            }
            appnames.Clear();
            appurls.Clear();
            repos.Clear();
            DataLoad(InstallDir+"apps.txt", "apps");
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
            foreach (KeyValuePair<string, string> keyValue in repos)
            {
                if (Type == "apps")
                {
                    appurls.Add(keyValue.Value);
                    appnames.Add(keyValue.Key);
                }
                else if (Type == "repos")
                {
                repourls.Add(keyValue.Value);
                reponames.Add(keyValue.Key);
                }
                file.Close();
                }
            }
        }
    }
