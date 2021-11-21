
using System;
using System.IO;
using System.Net;
public class SharpPackageManager
{

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        DataUpdate("C:\\Users\\yemas\\sources.txt", "repos");
        DataUpdate("C:\\Users\\yemas\\apps.txt", "apps");
    }
    public static void DataUpdate(string File, string Type)
    {
        using (StreamReader file = new StreamReader(File))
        {
            List<String> reponames = new List<String>();
            List<String> repourls = new List<String>();
            //string[] repourls;
            //string[] reponames;
            string ln;
            string[] ln1;

            while ((ln = file.ReadLine()) != null)
            {
                ln1 = ln.Split(",");
                repos.Add(ln1[0], ln1[1]);

            }
            foreach (KeyValuePair<string, string> keyValue in repos)
            {
                reponames.Add(keyValue.Value);
                repourls.Add(keyValue.Key);
            }
            file.Close();
            using (StreamReader rfile = new StreamReader(File))
            {
                List<String> appnames = new List<String>();
                List<String> appurls = new List<String>();
                //string[] repourls;
                //string[] reponames;
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
}