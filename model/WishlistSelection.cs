using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class WishlistSelection : TableEntity
    {
        public string UserId { get; set; }
        public string GameSelection { get; set; }
        public string GameTitle { get; set; }
        public string GameWeight { get; set; }
    }
}
