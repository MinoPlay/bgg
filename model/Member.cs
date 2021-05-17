using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public class Member : TableEntity
    {
        public string Initials { get; set; }
        public int Role { get; set; }
    }
}
