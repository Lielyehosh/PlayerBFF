using Common.Models.DbModels;
using Common.Utils;
using Common.Utils.DbModels;

namespace Common.Models
{
    public class GameDatabase: MongoDatabaseBase
    {
        public GameDatabase() : base()
        {
            AddCollectionDetails(new CollectionDetails<User>("user")
            {
                
            }) ;
        }
    }
}