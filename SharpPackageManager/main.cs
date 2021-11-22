
using System;
using System.IO;
using System.Net;
using System.Diagnostics;
public class SharpPackageManager
{
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();
    public static string InstallDir = "C:\\SPM\\config\\";

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        DataLoad(InstallDir + "sources.txt", "repos");
        DataLoad(InstallDir + "apps.txt", "apps");
        Console.WriteLine("Please choose your action!");
        Console.WriteLine("Install a package (i or install \n \n");
        Console.WriteLine("Update database (up or update)");
        Console.WriteLine(repourls[0] + " " + reponames[0]);
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
        //InstallPkg("steam");
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
