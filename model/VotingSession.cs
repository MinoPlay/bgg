using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public class VotingSession : TableEntity
    {
        public string SessionId { get; set; }
        public bool Active { get; set; }
    }
}
