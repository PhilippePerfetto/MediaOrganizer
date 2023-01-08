namespace TriPhotos.NitroShareLibrary
{
    internal class StoreVideo : AStoreMedia
    {
        internal static void Store(FileInfo srcMov, string ext)
        {
            Console.Write("Movie : " + srcMov.Name + "... ");
            FileInfo fileInfo = null;
            string text = "\\";
            string text2 = srcMov.Name;
            if (srcMov.Name[0] == '#' && srcMov.Name[9] == ' ')
            {
                text += srcMov.Name.Substring(1, 4);
                text += "\\";
                text += _month[Convert.ToInt16(srcMov.Name.Substring(5, 2))];
                text += "\\";
                text += srcMov.Name.Substring(7, 2);
                text += "\\";
            }
            else if (srcMov.Name.ToLower().EndsWith(".mts"))
            {
                if (srcMov.Name.Length == 18)
                {
                    text += srcMov.Name.Substring(0, 4);
                    text += "\\";
                    text += _month[Convert.ToInt16(srcMov.Name.Substring(4, 2))];
                    text += "\\";
                    text += srcMov.Name.Substring(6, 2);
                    text += "\\";
                    text2 = "#" + srcMov.Name.Substring(0, 8) + " " + srcMov.Name.Substring(8, 2) + "h" + srcMov.Name.Substring(10, 2) + "m" + srcMov.Name.Substring(12, 2) + "s.mts";
                }
                else if (srcMov.Name.Length < 18 && srcMov.Name.StartsWith("0"))
                {
                    text += srcMov.CreationTime.Year;
                    text += "\\";
                    text += _month[srcMov.CreationTime.Month];
                    text += "\\";
                    text += GetTwoDigit(srcMov.CreationTime.Day);
                    text += "\\";
                    text2 = "#" + GetTwoDigit(srcMov.CreationTime.Year) + GetTwoDigit(srcMov.CreationTime.Month) + GetTwoDigit(srcMov.CreationTime.Day) + " " + GetTwoDigit(srcMov.CreationTime.Hour) + "h" + GetTwoDigit(srcMov.CreationTime.Minute) + "m" + GetTwoDigit(srcMov.CreationTime.Second) + "s.mts";
                }
            }
            else
            {
                int num = srcMov.FullName.LastIndexOf(".");
                if (num > 0)
                {
                    fileInfo = new FileInfo(string.Concat(srcMov.FullName.AsSpan(0, num), ".JPG"));
                }
                if (!fileInfo?.Exists == true)
                {
                    fileInfo = new FileInfo(string.Concat(srcMov.FullName.AsSpan(0, num), ".THM"));
                }
                if (srcMov.Name.StartsWith("WP_"))
                {
                    short num2 = Convert.ToInt16(srcMov.Name.Substring(3, 4));
                    short num3 = Convert.ToInt16(srcMov.Name.Substring(7, 2));
                    short num4 = Convert.ToInt16(srcMov.Name.Substring(9, 2));
                    if (srcMov.LastWriteTime.Year != num2 || srcMov.LastWriteTime.Month != num3 || srcMov.LastWriteTime.Day != num4)
                    {
                        return;
                    }
                    text += num2;
                    text += "\\";
                    text += _month[num3];
                    text += "\\";
                    text += GetTwoDigit(num4);
                    text += "\\";
                    text2 = "#" + srcMov.Name.Substring(3, 8) + " " + GetTwoDigit(srcMov.LastWriteTime.Hour) + "h" + GetTwoDigit(srcMov.LastWriteTime.Minute) + "m" + GetTwoDigit(srcMov.LastWriteTime.Second) + "s" + srcMov.Extension;
                }
                else if (!fileInfo.Exists)
                {
                    DateTime lastWriteTime = srcMov.LastWriteTime;
                    text += lastWriteTime.Year;
                    text += "\\";
                    text += _month[lastWriteTime.Month];
                    text += "\\";
                    text += GetTwoDigit(lastWriteTime.Day);
                    text += "\\";
                    text2 = "#" + lastWriteTime.Year + GetTwoDigit(lastWriteTime.Month) + GetTwoDigit(lastWriteTime.Day) + " " + srcMov.Name;
                }
                else
                {
                    var date = SimpleRun.GetDate(fileInfo);
                    text += date.Value.Year;
                    text += "\\";
                    text += _month[Convert.ToInt16(date.Value.Month)];
                    text += "\\";
                    text += GetTwoDigit(date.Value.Day);
                    text += "\\";
                    text2 = GetNameFromDate(date.Value, ext);
                }
            }
            DirectoryInfo pathStartingWithDay = GetPathStartingWithDay(text);
            if (!pathStartingWithDay.Exists)
            {
                pathStartingWithDay.Create();
            }
            FileInfo fileInfo2 = new FileInfo(pathStartingWithDay.FullName + "\\" + text2);
            if (!fileInfo2.Exists)
            {
                srcMov.MoveTo(fileInfo2.FullName);
                DeleteIfPossible(fileInfo);
                Console.WriteLine("OK : " + fileInfo2.Name);
            }
            else if (fileInfo2.Length == srcMov.Length)
            {
                Console.WriteLine("Already exists --> deleted in TEMP");
                DeleteIfPossible(srcMov);
                DeleteIfPossible(fileInfo);
            }
            else
            {
                fileInfo2 = ObtainIndexedFile(fileInfo2);
                srcMov.MoveTo(fileInfo2.FullName);
                DeleteIfPossible(fileInfo);
                Console.WriteLine("Already exists but size is different. File moved with index to : " + fileInfo2.Name);
            }
        }
    }
}
