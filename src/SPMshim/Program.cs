using System;
using System.Diagnostics;
namespace SharpPackageManager {


    public class ShimInfo {
        public string Name { get; set; }
        public string Executable { get; set; }
        public string Icon { get; set; }
    }
    public class Shim {

        public static void Main(string[] args) {
            ShimInfo info = new ShimInfo();
            info.Name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            info.Executable = @"C:\SPM-APPS\"+info.Name+@"\"+info.Name+@".exe";
            info.Icon = @"C:\SPM-APPS\"+info.Name+@"\icon.ico";
            Console.Title = info.Name;
            Debug.WriteLine(info.Executable, info.Icon, info.Name);     
        }
    }
}