using Microsoft.Azure.Cosmos.Table;

namespace bgg
{
    public class GameInfo : TableEntity
    {
        public string GameId { get; set; }
        public string GameTitle { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string MinPlayers { get; set; }
        public string MaxPlayers { get; set; }
        public string MinPlaytime { get; set; }
        public string MaxPlaytime { get; set; }
        public string MinAge { get; set; }

        public string LanguageDependence { get; set; }
        public string AverageWeight { get; set; }
        public string Ranking { get; set; }
        public string GameState { get; set; }
    }
}
