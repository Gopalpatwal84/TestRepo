using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class ShowcaseDepartmentsJsonLoader : JsonLoader<ShowcaseDepartment>
    {
        public ShowcaseDepartmentsJsonLoader(IFileReader reader) : base(reader.ReadAllText("showcase-departments.json"))
        {
        }
    }
}
