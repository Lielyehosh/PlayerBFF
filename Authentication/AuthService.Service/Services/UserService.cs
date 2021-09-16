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
        
        public async Task<RegisterUserResponse> RegisterNewUserAsync(User user, CancellationToken ct)
        {
            if (user == null)
                return new RegisterUserResponse {Success = false, Error = "Bad message body"};
            _logger.LogDebug("Register new user {Username}", user.Username);
            
            var numberRegex = new Regex(@"^\d+$");
            // TODO - LY - add more validates to the request body and separate the validations for multiple uses 
            if (string.IsNullOrEmpty(user.Username))
                return new RegisterUserResponse {Success = false, Error = "User name can't be empty"};
            if (string.IsNullOrEmpty(user.IdNumber))
                return new RegisterUserResponse {Success = false, Error = "ID Number can't be empty"};
            if (!numberRegex.IsMatch(user.IdNumber))
                return new RegisterUserResponse {Success = false, Error = "ID Number have to contain only numbers"};

            var existingUser = await FindUserByIdNumberAsync(user.IdNumber, ct);
            if (existingUser != null) 
            {
                _logger.LogDebug("User with the same id number already exist - {IdNumber}", existingUser.IdNumber);
                return new RegisterUserResponse {Success = false, Error = "User with the same id number already exist"};
            }
            
            // validate passed
            try
            {
                await _dal.InsertOneAsync(user, ct);
                return new RegisterUserResponse() {Success = true, Error = ""};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert new document to db");
                return new RegisterUserResponse() {Success = false, Error = ex.Message};
            }
        }

        public async Task<User> FindUserByIdNumberAsync(string idNumber, CancellationToken ct)
        {
            var userColl = _dal.GetCollection<User>();
            var users = await userColl.Find(u => u.IdNumber == idNumber).ToListAsync(cancellationToken: ct);
            return users.Count > 0 ? users.FirstOrDefault() : null;
        }

        public bool TestM()
        {
            return true;
        }
    }
}