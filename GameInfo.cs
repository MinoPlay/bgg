using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class GameInfo : TableEntity
    {
        public string gameId { get; set; }
        public string gameTitle { get; set; }
        public string thumbnail { get; set; }
        public string description { get; set; }
        public string minplayers { get; set; }
        public string maxplayers { get; set; }
        public string minplaytime { get; set; }
        public string maxplaytime { get; set; }
        public string minage { get; set; }
    }
}
