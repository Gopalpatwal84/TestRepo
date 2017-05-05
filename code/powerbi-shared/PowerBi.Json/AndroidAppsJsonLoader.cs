using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class AndroidAppsJsonLoader : JsonLoader<AndroidApp>
    {
        public AndroidAppsJsonLoader(IFileReader reader) : base(reader.ReadAllText("android-apps.json"))
        {
        }
    }
}
