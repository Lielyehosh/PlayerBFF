using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BFF.Models.ViewModels;
using BFF.Service.Extensions;
using Common.Models.DbModels;
using Common.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BFF.Service.Controllers
{
    [DisableCors]
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : TableController<UserView,User>
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IMongoDal dal) : base(dal)
        {
            _logger = logger;
        }

        // public override UserView DbModelToViewModel(User user)
        // {
        //     if (user == null)
        //         return null;
        //     return new UserView
        //     {
        //         Id = user.Id,
        //         CreateAt = user.CreateAt,
        //         ModifyAt = user.ModifyAt,
        //         Username = user.Username,
        //         IdNumber = user.IdNumber,
        //         EmailAddress = user.EmailAddress
        //     };
        // }

        
        
        // [HttpGet("scheme")]
        // public Task<List<ViewModelScheme>> GetSchemeAsync(CancellationToken ct)
        // {
        //     _logger.LogDebug("Receive edit settings request - User-{User}" ,HttpContext.GetUserId());
        //     return Task.FromResult<IActionResult>(Ok());
        // }
        
        // [HttpGet("list")]
        // public async Task<List<UserView>> GetListAsync(CancellationToken ct)
        // {
        //     _logger.LogDebug("Receive edit settings request - Admin-{User}" ,HttpContext.GetUserId());
        //     var userColl = _dal.GetCollection<User>();
        //     var users = await userColl.Find(FilterDefinition<User>.Empty).ToListAsync(ct);
        //     return users.Select(DbModelToViewModel).ToList();
        // }
    }
}