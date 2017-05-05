using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CoursesJsonLoader : JsonLoader<Course>
    {
        public CoursesJsonLoader(IFileReader fileReader) : base(fileReader.ReadAllText("courses.json"))
        {
        }
    }
}
