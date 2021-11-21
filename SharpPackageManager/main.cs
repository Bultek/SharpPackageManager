
using System;
using System.IO;
using System.Net;
public class SharpPackageManager
{
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();
    public static string InstallDir = "C:\\temp\\";

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        DataLoad(InstallDir + "sources.txt", "repos");
        DataLoad(InstallDir + "apps.txt", "apps");
        Console.WriteLine("Please choose your action!");
        Console.WriteLine("Install a package (i or install \n \n");
        Console.WriteLine("Update database (up or update)");
        string action = Console.ReadLine();
        if (action == "i") // | action == "install")
        {
            Console.WriteLine("Which Package do you want to install?");
            //string Package = Console.ReadLine();
            InstallPkg(Console.ReadLine());
        }
        else if (action == "up") DataUpdate(InstallDir + "apps.txt", "apps"); //| action == "update");
        //InstallPkg("steam");
    }
    public static void InstallPkg(string Package)
    {
        //if (Package == null) Console.WriteLine("Please specify the package!");
        if (appnames.Contains(Package))
        {
            int pkgnumber = appnames.IndexOf(Package);
            Console.WriteLine(pkgnumber);
            Console.WriteLine("Downloading the package...");
            using (WebClient pkgdl = new WebClient())
            {
                pkgdl.DownloadFile(appurls[pkgnumber], "C:\\temp\\"+Package+".exe");
                // Param1 = Link of file
                // Param2 = Path to save
            }
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
            DataLoad(File, "apps");
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
                repourls.Add(keyValue.Key);
                reponames.Add(keyValue.Value);
                }
                file.Close();
                }
            }
        }
    }
