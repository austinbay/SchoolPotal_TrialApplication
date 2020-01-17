
using System;
using System.IO;
using ServiceStack.Text;

namespace GeneralHelper.Lib.Services
{
   
    public class CsvFileManager
    {
        public static DonwloadMpdel SaveCsvFile(object data)
        {
            if (data == null)
                throw new NullReferenceException("Object Provided is null");
            var csvData = CsvSerializer.SerializeToString(data);
            var exportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exports");
            if (!Directory.Exists(exportFolder))
                Directory.CreateDirectory(exportFolder);
            var fileName = Guid.NewGuid().ToString();
            var fileFullName = Path.Combine(exportFolder, $"{fileName}.csv");

            File.WriteAllText(fileFullName, csvData);
            return new DonwloadMpdel()
            {
                FileName = fileName
            };
        }

        public static Stream GetCsvStream(string filename)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exports", $"{filename}.csv");
            if (!File.Exists(file))
                throw new Exception("Tested");
            return File.OpenRead(file);
        }
    }

}
