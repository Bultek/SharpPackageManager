namespace UPDATER {
    public class UPDATER {
    public static string InstallDir = "C:\\SPM\\config\\";
    public static string InstallPath = "C:\\SPM\\";
        public static void Main(string[] args) {
            Console.WriteLine("Copying Files...");
            string[] upfiles = System.IO.Directory.GetFiles("C:\\SPM\\futureversion\\SPM");
            foreach (string upfile in upfiles)
            {
                string fileName = System.IO.Path.GetFileName(upfile);
                string destFile = System.IO.Path.Combine(InstallPath + fileName);
                System.IO.File.Copy(upfile, destFile, true);
            }
            System.IO.Directory.Move(InstallDir, "C:\\SPM\\oldconfig");
            System.IO.File.Copy("C:\\SPM\\oldconfig", InstallDir, true);
            System.IO.Directory.Delete("C:\\SPM\\oldconfig");
            System.IO.File.Delete("C:\\SPM\\futureversion\\unlock.txt");
            System.IO.File.Create(InstallDir + "clean.txt");
            Console.WriteLine("Please restart the app!");
        }
    }
}