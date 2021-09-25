using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using BFF.Models.ViewModels;
using BFF.Service.Extensions;
using Common.Models.DbModels;
using Common.Utils.DbModels;
using Common.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;


namespace BFF.Service.Controllers
{
    [DisableCors]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public abstract class TableController<TView,TModel> : Controller
        where TView : class, IViewModel, new()
        where TModel : class, IDbModel, new()
    {
        private readonly IMongoDal _dal;
        private readonly IMapper _mapper;

        protected TableController(IMongoDal dal)
        {
            _dal = dal;
            var config = new MapperConfiguration(cfg => { 
                cfg.CreateMap<TModel, TView>();
            });
            _mapper = config.CreateMapper();
        }
        
        [HttpGet("list")]
        public virtual async Task<List<TView>> GetListAsync(CancellationToken ct)
        {
            var collection = _dal.GetCollection<TModel>();
            var models = await collection.Find(FilterDefinition<TModel>.Empty).ToListAsync(ct);
            return models.Select(DbModelToViewModel).ToList();
        }
        
        // [HttpGet("scheme")]
        // public virtual async Task<List<string>> GetSchemeAsync(CancellationToken ct)
        // {
        //     var props = typeof(TView).GetProperties();
        //     var result = new List<string>();
        //     foreach (var p in props)
        //     {
        //         result.Add(p.Name);
        //     }
        //
        //     return result;
        // }
        
        public virtual TView DbModelToViewModel(TModel model)
        {
            if (model == null)
                return null;
            return _mapper.Map<TView>(model);
        }
    }
}