using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class VotingSessionEntry : TableEntity
    {
        public string VotingSessionEntryId { get; set; }
        public string VotingSessionnId { get; set; }
        public string GameId { get; set; }
    }
}
