using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.DbModels;
using Common.Utils;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthMS.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly MongoDal _dal;

        public UserService(
            ILogger<UserService> logger,
            MongoDal dal)
        {
            _logger = logger;
            _dal = dal;
        }

        public async Task<AuthUserResponse> LoginUserAsync(AuthUserRequest request, CancellationToken ct = default)
        {
            _logger.LogDebug("Auth user request via GRPC");
            var user = await FindUserByEmailAsync(request.Email, ct);
            // TODO - LY - implement a real validation and hashed the password
            if (user != null && request.Password == user.HashedPassword)
            {
                _logger.LogInformation("User authentication successes");
                return new AuthUserResponse {Success = true, Error = "", User = new UserData()
                {
                    Email = user.EmailAddress,
                    Username = user.Username
                }};
            }
            return new AuthUserResponse {Success = false};
        }

        public async Task<AuthUserResponse> RegisterNewUserAsync(User user, CancellationToken ct = default)
        {
            if (user == null)
                return new AuthUserResponse {Success = false, Error = "Bad message body"};
            _logger.LogDebug("Register new user {Username}", user.Username);
            
            var numberRegex = new Regex(@"^\d+$");
            // TODO - LY - add more validates to the request body and separate the validations for multiple uses 
            if (string.IsNullOrEmpty(user.Username))
                return new AuthUserResponse {Success = false, Error = "User name can't be empty"};
            if (string.IsNullOrEmpty(user.IdNumber))
                return new AuthUserResponse {Success = false, Error = "ID Number can't be empty"};
            if (!numberRegex.IsMatch(user.IdNumber))
                return new AuthUserResponse {Success = false, Error = "ID Number have to contain only numbers"};
            

            if (await FindUserByIdNumberAsync(user.IdNumber, ct) != null) 
            {
                _logger.LogDebug("User with the same id number already exist - {IdNumber}", user.IdNumber);
                return new AuthUserResponse {Success = false, Error = "User with the same id number already exist"};
            }
            
            if (await FindUserByEmailAsync(user.EmailAddress, ct) != null) 
            {
                _logger.LogDebug("User with the same email address already exist - {Email}", user.EmailAddress);
                return new AuthUserResponse {Success = false, Error = "User with the same email already exist"};
            }
            
            if (await FindUserByUsernameAsync(user.Username, ct) != null) 
            {
                _logger.LogDebug("User with the same username already exist - {Username}", user.Username);
                return new AuthUserResponse {Success = false, Error = "User with the same username already exist"};
            }
            
            // validate passed
            try
            {
                await _dal.InsertOneAsync(user, ct);
                return new AuthUserResponse() {Success = true, Error = "", User = new UserData()
                {
                    Email = user.EmailAddress,
                    Username = user.Username
                }};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert new document to db");
                return new AuthUserResponse() {Success = false, Error = ex.Message};
            }
        }

        public async Task<User> FindUserByIdNumberAsync(string idNumber, CancellationToken ct = default)
        {
            var userColl = _dal.GetCollection<User>();
            var users = await userColl.Find(u => u.IdNumber == idNumber).ToListAsync(cancellationToken: ct);
            return users.Count > 0 ? users.FirstOrDefault() : null;
        }

        public async Task<User> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var userColl = _dal.GetCollection<User>();
            var users = await userColl.Find(u => u.EmailAddress == email).ToListAsync(cancellationToken: ct);
            return users.Count > 0 ? users.FirstOrDefault() : null;
        }

        public async Task<User> FindUserByUsernameAsync(string username, CancellationToken ct = default)
        {
            var userColl = _dal.GetCollection<User>();
            var users = await userColl.Find(u => u.Username == username).ToListAsync(cancellationToken: ct);
            return users.Count > 0 ? users.FirstOrDefault() : null;
        }
    }
}