using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public class VotingSessionEntry : TableEntity
    {
        public string VotingSessionEntryId { get; set; }
        public string VotingSessionId { get; set; }
        public string GameId { get; set; }
        public string GameTitle { get; set; }
    }
}
