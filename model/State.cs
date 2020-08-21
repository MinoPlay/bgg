using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class State : TableEntity
    {
        public string availableState { get; set; }
    }
}