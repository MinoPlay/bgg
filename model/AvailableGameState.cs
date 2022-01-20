using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public class AvailableGameState : TableEntity
    {
        public string State { get; set; }
    }
}
