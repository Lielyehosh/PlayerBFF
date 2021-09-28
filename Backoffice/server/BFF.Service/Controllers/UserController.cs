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
    }
}