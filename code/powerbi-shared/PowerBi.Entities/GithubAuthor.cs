using System;

namespace PowerBI.Entities
{
    [Serializable]
    public class GithubAuthor
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }
    }
}
