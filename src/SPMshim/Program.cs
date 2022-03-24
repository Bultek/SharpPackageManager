using System;
using System.Diagnostics;
using System.Windows;
namespace SharpPackageManager {


    public class ShimInfo {
        public string Name { get; set; }
        public string Executable { get; set; }    }
    public class Shim {

        public static string exec;
        public static void Main(string[] args) {
            ShimInfo info = new ShimInfo();
            info.Name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (File.Exists(@"C:\SPM-APPS\"+info.Name+@"\"+info.Name+@".exe")) {
               exec = @"C:\SPM-APPS\"+info.Name+@"\"+info.Name+@".exe";
            }
            else {
                if (File.Exists(@"C:\SPM-APPS\"+@info.Name+@"\shimpath.spmdata") && !File.Exists(@"C:\SPM-APPS\"+@info.Name+@"\"+@info.Name+@".exe")) {
                    using (var shimdata = new StreamReader(@"C:\SPM-APPS\"+@info.Name+@"\shimpath.spmdata")) {
                        exec = shimdata.ReadLine();
                    }
                }
                else throw new FileNotFoundException("ShimData File Not Found");
            }
            info.Executable = exec;
            Console.Title = info.Name;
            Debug.WriteLine(info.Executable, info.Name);
            if (File.Exists(info.Executable)) {
                Debug.WriteLine("Running: "+info.Executable);
                System.Diagnostics.Process.Start(info.Executable);
            }
            else {
                Debug.WriteLine("File Not Found");
//                Forms.MessageBox.Show("File Not Found");
                throw new FileNotFoundException("Executable File wasn't found.");
            }
        }
    }
}