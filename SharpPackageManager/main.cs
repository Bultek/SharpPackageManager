﻿
using System;
using System.IO;
using System.Net;
public class SharpPackageManager
{
    //public string[] repourls = new string[99999];
    //public string[] reponames = new string[99999];
    
    public static void Main(string[] args)
    {
        Dictionary<string, string> repos = new Dictionary<string, string>();
        using (StreamReader file = new StreamReader("C:\\Users\\yemas\\sources"+".txt"))
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
            using (StreamReader file = new StreamReader("C:\\Users\\yemas\\sources" + ".txt"))
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
                Console.WriteLine("Welcome to SPM");
            Console.WriteLine("Please select your action! \n");




        }
    }
}
