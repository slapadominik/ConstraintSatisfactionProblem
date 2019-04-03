using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}