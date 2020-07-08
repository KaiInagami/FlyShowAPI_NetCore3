using System;
using System.IO;

namespace FlyshowVegetablesAPI.Utilities
{
    public class FileManager
    {
        public static void Writer(string bulletin, string fileName, string physicalPath)
        {
            if (string.IsNullOrEmpty(physicalPath))
            {
                throw new Exception("Null path.");
            }

            try
            {
                if (!System.IO.Directory.Exists(physicalPath))
                {
                    System.IO.Directory.CreateDirectory(physicalPath);
                }

                FileStream fileStream;
                if (!File.Exists(physicalPath + fileName))
                {
                    fileStream = new FileStream(physicalPath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                else
                {
                    fileStream = new FileStream(physicalPath + fileName, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite);
                }

                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(bulletin);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Read(string fileName, string physicalPath)
        {
            string bulletin;
            try
            {
                if (!File.Exists(physicalPath + fileName))
                {
                    return "";
                }

                FileStream fileStream = new FileStream(physicalPath + fileName, FileMode.Open);
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    bulletin = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bulletin;
        }
    }
}
