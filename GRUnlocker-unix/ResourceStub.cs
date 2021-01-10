using System.Globalization;
using System.IO;
using System.Resources;

namespace GRUnlocker_unix
{
    public class ResourceStub : ResourceManager
    {
        public override object GetObject(string name, CultureInfo culture)
        {
            return File.ReadAllBytes($"Resources/{name}.sav");
        }
    }
}