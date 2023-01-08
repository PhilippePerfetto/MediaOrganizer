namespace TriPhotos.NitroShareLibrary
{
    internal abstract class AStoreMedia
    {
        protected static string[] _month = new string[13]
        {
        "", "01-Janvier", "02-Février", "03-Mars", "04-Avril", "05-Mai", "06-Juin", "07-Juillet",
            "08-Août", "09-Septembre", "10-Octobre", "11-Novembre", "12-Décembre"
        };

        internal static DirectoryInfo? OutputDirectory { private get; set; }

        protected static void DeleteIfPossible(FileInfo file)
        {
            if (file?.Exists == true)
                file.Delete();
        }

        protected static string GetTwoDigit(int number)
        {
            if (number <= 9)
            {
                return "0" + number;
            }
            return string.Concat(number);
        }

        protected static string GetNameFromTag(string dt, string ext)
        {
            return "#" + dt[..4] + dt.Substring(5, 2) + dt.Substring(8, 2) + " " + dt.Substring(11, 2) + "h" + dt.Substring(14, 2) + "m" + dt.Substring(17, 2) + "s." + ext;
        }

        protected static string GetNameFromDate(DateTime dt, string ext)
        {
            return "#" + dt.Year + GetTwoDigit(dt.Month) + GetTwoDigit(dt.Day) + " "
                + GetTwoDigit(dt.Hour) + "h" + GetTwoDigit(dt.Minute) + "m" + GetTwoDigit(dt.Second) + "s." + ext;
        }

        protected static FileInfo ObtainIndexedFile(FileInfo destFile)
        {
            int num = destFile.Name.LastIndexOf(".");
            string text = destFile.Name.Substring(0, num);
            string text2 = destFile.Name.Substring(num + 1);
            int num2 = 0;
            var fileInfo = new FileInfo(destFile.Directory.FullName + "\\" + text + "-" + num2 + "." + text2);
            while (fileInfo.Exists)
            {
                fileInfo = new FileInfo(destFile.Directory.FullName + "\\" + text + "-" + num2 + "." + text2);
                num2++;
            }
            return fileInfo;
        }

        protected static DirectoryInfo GetPathStartingWithDay(string pathDir)
        {
            var destDir = new DirectoryInfo(OutputDirectory.FullName + "/" + pathDir);
            if (destDir.Exists)
                return destDir;

            if (destDir.Parent?.Exists == true)
            {
                DirectoryInfo directoryInfo = destDir.Parent.GetDirectories().FirstOrDefault((DirectoryInfo x) => x.Name.StartsWith(destDir.Name));
                if (directoryInfo != null)
                {
                    return directoryInfo;
                }
            }
            return new DirectoryInfo(OutputDirectory.FullName + "/" + pathDir);
        }
    }
}
