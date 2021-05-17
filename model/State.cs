using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public class State : TableEntity
    {
        public string availableState { get; set; }
    }
}