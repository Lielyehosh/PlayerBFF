using System.Threading;
using System.Threading.Tasks;
using AuthMS;
using AuthMS.Services;
using AuthService.Models;
using Common.Models;
using Common.Models.DbModels;
using Common.Tests.Mock;
using Common.Utils;
using Common.Utils.Settings;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AuthService.Tests
{
    public class UserServiceUnitTests
    {
        private MongoDalMock _dal;
        private IUserService _userService;

        [SetUp]
        public async Task Setup()
        {
            _dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
            _userService = new UserService(TestsCommon.Logger<UserService>(), _dal);
        }

        [Test]
        [TestCase("","","", ExpectedResult = false)]
        [TestCase("","Sharon@gmail.com","", ExpectedResult = false)]
        [TestCase("","Sharon@gmail.com","Sharon", ExpectedResult = false)]
        [TestCase("12312312a","Sharon@gmail.com","Sharon", ExpectedResult = false)]
        [TestCase("123123123","aaa@gmail.com","Sharon", ExpectedResult = true)]
        public async Task<bool> RegisterNewUserTest(string idNumber, string emailAddress, string userName)
        {
            var ct = new CancellationToken();
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = userName,
                IdNumber = idNumber,
                EmailAddress = emailAddress
            }, ct);
            return result.Success;
        }

        [TearDown]
        public async Task Cleanup()
        {
            _dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
        }
    }
}