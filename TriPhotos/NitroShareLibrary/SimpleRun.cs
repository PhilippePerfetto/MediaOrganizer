using MetadataExtractor;
using System;

namespace TriPhotos.NitroShareLibrary
{
    /// <summary>
    /// Date/Time ; Date/Time Original ; Date/Time Digitized ; GPS Date Stamp (uniquement date, pas d'heure) ; 
    /// </summary>
    internal static class SimpleRun
    {
        internal static DateTime? GetDate(FileInfo file)
        {
            List<string> dates = new();
            List<DateTime> dts = new();

            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);

            var all = directories.SelectMany(x => x.Tags).ToList();
            var model = all.FirstOrDefault(x => x.Name == "Model");

            if (model?.Name != null)
            {
                // "Date/Time Digitized"
                // "Date/Time Original"
                // "Date/Time"
                switch (model.Description)
                {
                    case "V2050":
                        return GetDateFromExif(all.FirstOrDefault(x => x.Name == "Date/Time Digitized")?.Description);
                    case "Canon EOS 600D": 
                        return GetDateFromExif(all.FirstOrDefault(x => x.Name == "Date/Time Digitized")?.Description);
                }
            }

            var digitized = GetDateFromExif(all.FirstOrDefault(x => x.Name == "Date/Time Digitized")?.Description);
            var original = GetDateFromExif(all.FirstOrDefault(x => x.Name == "Date/Time Original")?.Description);
            var basic = GetDateFromExif(all.FirstOrDefault(x => x.Name == "Date/Time")?.Description);
            return digitized != null ? digitized : (original != null ? original : basic);
        }

        private static DateTime? GetDateFromExif(string exifDateString)
        {
            if (exifDateString == null)
                return null;

            var slice = exifDateString.AsSpan();

            var y = int.Parse(slice.Slice(0, 4));
            var m = int.Parse(slice.Slice(5, 2));
            var d = int.Parse(slice.Slice(8, 2));
            var h = int.Parse(slice.Slice(11, 2));
            var mn = int.Parse(slice.Slice(14, 2));
            var s = int.Parse(slice.Slice(17, 2));

            var dt = new DateTime(y, m, d, h, mn, s);
            return dt;
        }

        internal static void DebugForFile(FileInfo file)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                    Console.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
            }

            var all = directories.SelectMany(x => x.Tags).ToList();
            Console.WriteLine(all.Count);
        }
    }
}
