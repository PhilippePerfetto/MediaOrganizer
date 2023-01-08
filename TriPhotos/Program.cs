/*
RepositoryPathManager.ROOT_REPOSITORY = Settings.Default.Path;
AddFilesManager addFilesManager = new AddFilesManager();
addFilesManager.Run();
*/

using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using TriPhotos.NitroShareLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        var fileEntry = Assembly.GetEntryAssembly()?.Location;

        if (fileEntry != null)
        {
            var dirEntry = new FileInfo(fileEntry).Directory;
            DirectoryInfo temp = new($"{dirEntry.FullName}\\TEMP");
            DirectoryInfo output = new($"{dirEntry.FullName}\\_ALL_PICS_");

            if (args.Length == 1) { output = new(args[0]); }

            Console.WriteLine($"Input  files in : {temp.FullName}");
            Console.WriteLine($"Output files in : {output.FullName}");
            Console.WriteLine("Start...");

            // SimpleRun.DebugForFile(new FileInfo(@"D:\Prog\Gits\TriPhotos\TriPhotos\TriPhotos\bin\Debug\net7.0\TEMP\IMG_8765-Fernanda1.JPG"));

            AStoreMedia.OutputDirectory = output;

            FileBrowser fb = new();
            fb.Run(temp, output);

            // Remove Empty directories
            var subDirs = temp.GetDirectories("*", SearchOption.AllDirectories);
            var subDirsEmpty = subDirs.Where(x => x.GetFiles("*").Count() == 0).ToList();
            foreach (var sub in subDirsEmpty)
                if (sub.Exists)
                    try
                    {
                        sub.Delete(true);
                    }
                    catch (Exception) { }
        }
    }
}