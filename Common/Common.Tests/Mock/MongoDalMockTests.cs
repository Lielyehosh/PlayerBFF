using System.Threading.Tasks;
using Common.Models;
using NUnit.Framework;

namespace Common.Tests.Mock
{
    public class MongoDalMockTests
    {
        private MongoDalMock dal;

        [SetUp]
        public async Task SetUp()
        {
            dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
        }
        
    }
}