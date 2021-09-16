using System.Threading.Tasks;
using Common.Models;
using NUnit.Framework;

namespace Common.Tests.Mock
{
    public class MongoDalMockTests
    {
        private MongoDalMock _dal;

        [SetUp]
        public async Task SetUp()
        {
            _dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
        }
        
    }
}