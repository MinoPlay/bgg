using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class GameState : TableEntity
    {
        public string gameId { get; set; }
        public string gameState { get; set; }
    }
}