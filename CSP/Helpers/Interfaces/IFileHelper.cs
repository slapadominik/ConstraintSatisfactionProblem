using System.Collections.Generic;

namespace CSP.Helpers.Interfaces
{
    public interface IFileHelper
    {
        List<string> ReadFile(string path);

        void WriteToFile(string text, string fileName, string path);
    }
}