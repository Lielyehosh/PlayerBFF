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
        [TestCase("","","", ExpectedResult = false, TestName = "Register new user with empty fields")]
        [TestCase("","Sharon@gmail.com","", ExpectedResult = false, TestName = "Register new user with only email address")]
        [TestCase("","Sharon@gmail.com","Sharon", ExpectedResult = false, TestName = "Register new user with only username and email address")]
        [TestCase("12312312a","Sharon@gmail.com","Sharon", ExpectedResult = false, TestName = "Register new user with invalid id number")]
        [TestCase("123123123","aaa@gmail.com","Sharon", ExpectedResult = true, TestName = "Register new user with valid id number & email and username")]
        public async Task<bool> RegisterNewUserTest(string idNumber, string emailAddress, string userName)
        {
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = userName,
                IdNumber = idNumber,
                EmailAddress = emailAddress
            });
            return result.Success;
        }

        [Test]
        [TestCase("123123123", "Sharon1@gmail.com", "Sharon1", ExpectedResult = false, TestName = "Register user with existing id number")]
        [TestCase("123123124", "Sharon1@gmail.com", "Sharon1", ExpectedResult = true, TestName = "Register user with non-existing id number")]
        [TestCase("123123124", "Sharon@gmail.com", "Sharon1", ExpectedResult = false, TestName = "Register user with existing email address")]
        [TestCase("123123124", "Sharon1@gmail.com", "Sharon1", ExpectedResult = true, TestName = "Register user with non-existing email address")]
        [TestCase("123123124", "Sharon1@gmail.com", "Sharon", ExpectedResult = false, TestName = "Register user with existing username")]
        [TestCase("123123124", "Sharon1@gmail.com", "Sharon1", ExpectedResult = true, TestName = "Register user with non-existing username")]
        public async Task<bool> RegisterUserWithExistingFieldTest(string idNumber, string emailAddress, string userName)
        {
            await _userService.RegisterNewUserAsync(new User()
            {
                Username = "Sharon",
                IdNumber = "123123123",
                EmailAddress = "Sharon@gmail.com"
            });
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = userName,
                IdNumber = idNumber,
                EmailAddress = emailAddress
            });
            return result.Success;
        }


        [TearDown]
        public async Task Cleanup()
        {
            _dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
        }
    }
}