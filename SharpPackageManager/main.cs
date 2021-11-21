
using System;
using System.IO;
using System.Net;
public class SharpPackageManager
{

    public static Dictionary<string, string> repos = new Dictionary<string, string>();
    public static void Main(string[] args)
    {
        //
    }
    public static void FileReader(string File)
    {
        using (StreamReader file = new StreamReader("C:\\Users\\The Encoder\\Documents\\GitHub\\SharpPackageManager\\test.txt"))
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
                    appnames.Add(keyValue.Value);
                    appurls.Add(keyValue.Key);


                }
                file.Close();




            }
        }
    }
}
