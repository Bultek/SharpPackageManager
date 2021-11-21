
using System;
using System.IO;
using System.Net;
public class SharpPackageManager
{
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();
    public static string InstallDir = "C:\\";

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        DataUpdate(InstallDir + "sources.txt", "repos");
        DataUpdate(InstallDir + "apps.txt", "apps");
        Console.WriteLine("Please choose your action!");
        InstallPkg("steam");
    }
    public static void InstallPkg(string Package)
    {
        //if (Package == null) Console.WriteLine("Please specify the package!");
        if (appnames.Contains(Package)) //DONT WRITE "==true", YOU STUPID SHIT
        {
            int pkgnumber = appnames.IndexOf(Package);
            Console.WriteLine(pkgnumber);
            using (WebClient pkgdl = new WebClient())
            {


                pkgdl.DownloadFile(appurls[pkgnumber], "C:\\temp\\"+Package+".exe");

                // Param1 = Link of file
                // Param2 = Path to save
            }
        }
    }
    public static void DataUpdate(string File, string Type)
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
