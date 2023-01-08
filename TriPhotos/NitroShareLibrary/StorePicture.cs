using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriPhotos.NitroShareLibrary
{
    internal class StorePicture : AStoreMedia
    {
        internal static void Store(FileInfo srcJpg)
        {
            Console.Write("Jpg : " + srcJpg.Name + "... ");
            string text = "\\";
            string text2 = srcJpg.Name;

            var date = SimpleRun.GetDate(srcJpg);

            if (date != null)
            {
                text += date.Value.Year;
                text += "\\";
                text += _month[date.Value.Month];
                text += "\\";
                text += GetTwoDigit(date.Value.Day);
                text += "\\";
                text2 = GetNameFromDate(date.Value, "jpg");
            }
            else if (srcJpg.Name[0] == '#' && srcJpg.Name[9] == ' ')
            {
                text += srcJpg.Name.Substring(1, 4);
                text += "\\";
                text += _month[Convert.ToInt16(srcJpg.Name.Substring(5, 2))];
                text += "\\";
                text += srcJpg.Name.Substring(7, 2);
                text += "\\";
            }

            DirectoryInfo pathStartingWithDay = GetPathStartingWithDay(text);
            if (!pathStartingWithDay.Exists)
            {
                pathStartingWithDay.Create();
            }

            FileInfo fileInfo = new FileInfo(pathStartingWithDay.FullName + "\\" + text2);
            if (!fileInfo.Exists)
            {
                srcJpg.MoveTo(fileInfo.FullName);
                Console.WriteLine("OK : " + fileInfo.Name);
            }
            else if (fileInfo.Length == srcJpg.Length)
            {
                Console.WriteLine("Already exists --> deleted in TEMP");
                srcJpg.Delete();
            }
            else
            {
                fileInfo = ObtainIndexedFile(fileInfo);
                srcJpg.MoveTo(fileInfo.FullName);
                Console.WriteLine("Already exists but size is different. File moved with index to : " + fileInfo.Name);
            }
        }
    }
}
