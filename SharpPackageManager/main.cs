
using System;
using System.IO;
using System.Net;
public class SharpPackageManager
{
    public static List<String> reponames = new List<String>();
    public static List<String> repourls = new List<String>();
    public static List<String> appnames = new List<String>();
    public static List<String> appurls = new List<String>();

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        DataUpdate("C:\\Users\\yemas\\sources.txt", "repos");
        DataUpdate("C:\\Users\\yemas\\apps.txt", "apps");
        Console.WriteLine("Please choose your action!");
        Console.WriteLine(appnames[1]+" "+appurls[1]+" "+reponames[1]+" "+repourls[1]);

    }
    public static void DataUpdate(string File, string Type)
    {
        using (StreamReader file = new StreamReader(File))
        {

            string ln2;
            string[] ln3;
            while ((ln2 = file.ReadLine()) != null)
               {
                ln3 = ln2.Split(",");
                repos.Add(ln3[0], ln3[1]);
               }
            foreach (KeyValuePair<string, string> keyValue in repos)
            {
                if (Type == "apps")
                {
                    appnames.Add(keyValue.Value);
                    appurls.Add(keyValue.Key);
                }
                else if (Type == "repos")
                {
                reponames.Add(keyValue.Key);
                repourls.Add(keyValue.Value);
                }
                file.Close();
                }
            }
        }
    }
