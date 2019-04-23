using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSP.Exceptions;
using CSP.Helpers.Interfaces;

namespace CSP.Helpers
{
    public class FileHelper : IFileHelper
    {
        public List<string> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            return File.ReadAllLines(path).ToList();
        }

        public void WriteToFile(string text, string fileName, string path)
        {
            if (File.Exists(path+fileName))
            {
                throw new FileAlreadyExistsException($"File {fileName} already exists in {path}");
            }
            File.WriteAllText(path + fileName, text, Encoding.UTF8);
        }
    }
}