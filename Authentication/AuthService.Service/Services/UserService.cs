using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AuthMS.Utils;
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

        public async Task<AuthUserResponse> LoginUserAsync(string email, string password, CancellationToken ct = default)
        {
            _logger.LogDebug("Auth user request via GRPC");
            var user = await FindUserByEmailAsync(email, ct);
            
            // TODO - LY - implement a real validation and hashed the password
            if (user != null && password == user.HashedPassword)
            {
                _logger.LogInformation("User authentication successes");
                return new AuthUserResponse {Success = true, Error = "", User = new UserData()
                {
                    Email = user.EmailAddress,
                    Username = user.Username,
                    Id = user.Id
                }};
            }
            return new AuthUserResponse {Success = false};
        }

        public async Task<AuthUserResponse> RegisterNewUserAsync(User user, CancellationToken ct = default)
        {
            if (user == null)
                return new AuthUserResponse {Success = false, Error = "Bad message body"};
            _logger.LogDebug("Register new user {Username}", user.Username);
            
            // TODO - LY - add more validates to the request body and separate the validations for multiple uses 
            if (!Validator.ValidateEmail(user.EmailAddress))
                return new AuthUserResponse {Success = false, Error = "Invalid email address"};
            if (!Validator.ValidatePassword(user.HashedPassword))
                return new AuthUserResponse {Success = false, Error = "Invalid password"};
            
            
            // TODO - its a temporary validate for existing fields, need to implement a better way for this validate
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
                user = await _dal.InsertOneAsync(user, ct);
                return new AuthUserResponse() {Success = true, Error = "", User = new UserData()
                {
                    Email = user.EmailAddress,
                    Username = user.Username,
                    Id = user.Id
                }};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert new document to db");
                return new AuthUserResponse() {Success = false, Error = ex.Message};
            }
        }

        public async Task<AuthUserResponse> ResetPwAsync(string userId, string pw, CancellationToken ct)
        {
            var userColl = _dal.GetCollection<User>();
            
            var user = await userColl.FindOneAndUpdateAsync(u => u.Id == userId,
                Builders<User>.Update
                    .Set(u => u.HashedPassword, pw), cancellationToken: ct);
            if (user == null)
            {
                _logger.LogError("User not found id - {UserId}", userId);
                return new AuthUserResponse()
                {
                    Error = "user not found",
                    Success = false,
                    User = null
                };
            }

            return new AuthUserResponse()
            {
                Error = "",
                Success = true,
                User = new UserData()
                {
                    Email = user.EmailAddress,
                    Username = user.Username,
                    Id = user.Id
                }
            };

        }


        public async Task<User> FindUserByIdAsync(string userId, CancellationToken ct = default)
        {
            var userColl = _dal.GetCollection<User>();
            var users = await userColl.Find(u => u.Id == userId).ToListAsync(cancellationToken: ct);
            return users.Count > 0 ? users.FirstOrDefault() : null;
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
            var users = await userColl.Find(u => u.EmailAddress == email.ToLower()).ToListAsync(cancellationToken: ct);
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