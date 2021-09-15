using Common.Utils;
using Microsoft.Extensions.Logging;

namespace Common.Models
{
    public class GameDal : MongoDal
    {
        private readonly ILogger<GameDal> _logger;
        
        public GameDal(MongoDatabaseBase db, ILogger<GameDal> logger) : base(db)
        {
            _logger = logger;
        }
    }
}