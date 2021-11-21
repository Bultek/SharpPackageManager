
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
            string ln;
            string[] ln1;

            while ((ln = file.ReadLine()) != null)
            {
                ln1 = ln.Split(", ");
                repos.Add(ln1[0], ln1[1]);
            }
            foreach (KeyValuePair<string, string> keyValue in repos)
            {
                
            }
            file.Close();
        }
    }
}
