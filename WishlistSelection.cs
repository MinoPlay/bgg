using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace bgg
{
    public class WishlistSelection : TableEntity
    {
        public string UserId { get; set; }
        // since table storage doesn't support arrays will store as ',' seperated string
        public string GamesSelections { get; set; }
    }
}
