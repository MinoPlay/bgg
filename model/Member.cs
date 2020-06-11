using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class Member : TableEntity
    {
        public string Initials { get; set; }
        public MemberRole Role { get; set; }
    }
}
