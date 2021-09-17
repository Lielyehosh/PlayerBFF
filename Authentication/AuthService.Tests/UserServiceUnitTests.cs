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
        private User ExistingUser { get; set; }

        [SetUp]
        public async Task Setup()
        {
            _dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
            _userService = new UserService(TestsCommon.Logger<UserService>(), _dal);
            // Create a temp user for the tests
            ExistingUser = new User()
            {
                Username = "Test",
                IdNumber = "123456789",
                EmailAddress = "test@gmail.com",
                HashedPassword = "1234",
            };
            await _userService.RegisterNewUserAsync(ExistingUser);
        }
        
        
        [Test]
        [TestCase("", TestName = "Empty email address", ExpectedResult = false)]
        [TestCase("Test@gmail.com", TestName = "Existing email address", ExpectedResult = false)]
        [TestCase("test@gmail.com", TestName = "Existing email address (lower case)", ExpectedResult = false)]
        [TestCase("TEST@gmail.com", TestName = "Existing email address (upper case)", ExpectedResult = false)]
        [TestCase("TEST1@gmail.com", TestName = "Valid email", ExpectedResult = true)]
        public async Task<bool> RegisterUser_EmailTest(string email)
        {
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = "Test2",
                IdNumber = "111111111",
                EmailAddress = email,
                HashedPassword = "1234"
            });
            return result.Success;
        }
        
        [Test]
        [TestCase("", TestName = "Empty password", ExpectedResult = false)]
        [TestCase(null, TestName = "NULL password", ExpectedResult = false)]
        [TestCase("1234", TestName = "Valid password", ExpectedResult = true)]
        public async Task<bool> RegisterUser_PasswordTest(string password)
        {
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = "Test2",
                IdNumber = "111111111",
                EmailAddress = "Test2@gmail.com",
                HashedPassword = password
            });
            return result.Success;
        }
        
        [Test]
        [TestCase("", TestName = "Empty username", ExpectedResult = false)]
        [TestCase("Test", TestName = "Existing username", ExpectedResult = false)]
        [TestCase("test", TestName = "Existing username (lower case)", ExpectedResult = true)]
        [TestCase("TEST", TestName = "Existing username (upper case)", ExpectedResult = true)]
        [TestCase("test2", TestName = "Valid username", ExpectedResult = true)]
        public async Task<bool> RegisterUser_UsernameTest(string username)
        {
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = username,
                IdNumber = "111111111",
                EmailAddress = "Test2@gmail.com",
                HashedPassword = "1234"
            });
            return result.Success;
        }
        
        [Test]
        [TestCase("", TestName = "Empty id number", ExpectedResult = false)]
        [TestCase("123456789", TestName = "Existing id number", ExpectedResult = false)]
        [TestCase("12312312a", TestName = "Invalid id number", ExpectedResult = false)]
        [TestCase("12n3123125", TestName = "Invalid id number", ExpectedResult = false)]
        [TestCase("111111111", TestName = "Valid id number", ExpectedResult = true)]
        public async Task<bool> RegisterUser_IdNumberTest(string idNumber)
        {
            var result = await _userService.RegisterNewUserAsync(new User()
            {
                Username = "Test2",
                IdNumber = idNumber,
                EmailAddress = "Test2@gmail.com",
                HashedPassword = "1234"
            });
            return result.Success;
        }
        
        
        //
        //
        // [Test]
        // [TestCase("email")]
        // public async Task<bool> LoginUserTests(string email, string password)
        // {
        //     var result = await _userService.LoginUserAsync(email,password);
        //     return result.Success;
        // }

        [TearDown]
        public async Task Cleanup()
        {
            _dal = await MongoDalMock.GetMockMongoDalAsync<GameDatabase>();
        }
    }
}