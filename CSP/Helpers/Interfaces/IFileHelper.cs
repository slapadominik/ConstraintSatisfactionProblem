using System.Collections.Generic;

namespace CSP.Helpers.Interfaces
{
    public interface IFileHelper
    {
        List<string> ReadFile(string path);
    }
}