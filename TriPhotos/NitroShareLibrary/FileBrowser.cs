namespace TriPhotos.NitroShareLibrary
{
    internal class FileBrowser
    {
        internal void Run(DirectoryInfo temp, DirectoryInfo output)
        {
            if (!temp.Exists)
            {
                temp.Create();
            }
            BrowseDirectories(temp);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Work done. Press a key.");
            Console.ReadKey();
        }

        private static IEnumerable<FileInfo> GetVideoFiles(DirectoryInfo dir)
        {
            var files = dir.GetFiles("*.mov");
            var files2 = dir.GetFiles("*.mkv");
            var files3 = dir.GetFiles("*.avi");
            var files4 = dir.GetFiles("*.mpg");
            var files5 = dir.GetFiles("*.mp4");
            var files6 = dir.GetFiles("*.mts");
            return files.Concat(files2).Concat(files3).Concat(files4)
                .Concat(files5)
                .Concat(files6);
        }

        private void BrowseDirectories(DirectoryInfo dir)
        {
            foreach (FileInfo videoFile in GetVideoFiles(dir))
            {
                try
                {
                    int num = videoFile.Name.LastIndexOf(".");
                    StoreVideo.Store(videoFile, videoFile.Name.Substring(num + 1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't manage movie : " + videoFile.FullName + "   " + ex.Message);
                }
            }
            FileInfo[] files = dir.GetFiles("*.j*");
            foreach (FileInfo fileInfo in files)
            {
                try
                {
                    StorePicture.Store(fileInfo);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Couldn't manage picture : " + fileInfo.FullName + "   " + ex2.Message);
                }
            }

            DirectoryInfo[] directories = dir.GetDirectories();
            foreach (DirectoryInfo sub in directories)
            {
                BrowseDirectories(sub);
            }
        }
    }
}
