using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class DepartmentsJsonLoader : JsonLoader<Department>
    {
        public DepartmentsJsonLoader(IFileReader reader) : base(reader.ReadAllText("departments.json"))
        {
        }
    }
}
