using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using BFF.Service.Exceptions;
using Common.Models.Table;
using Common.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BFF.Service.Controllers
{
    [DisableCors]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public abstract class TableController<TView, TModel> : Controller
        where TView : class, IViewModel, new()
        where TModel : class, IDbModel, new()
    {
        private readonly IMongoDal _dal;
        private readonly IMapper _mapper;
        private readonly Dictionary<string,TableFormField> _fieldsById = new Dictionary<string, TableFormField>();

        protected TableController(IMongoDal dal)
        {
            _dal = dal;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TModel, TView>();
                cfg.CreateMap<TView, TModel>();
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
        
        [HttpGet("form")]
        public virtual async Task<TableForm> GetTableFormAsync(CancellationToken ct)
        {
            var form = ResolveFieldsFromType(typeof(TView), _fieldsById);
            return form;
        }
        
        public bool EnumAsString { get; set; } = true;

        protected virtual TView DbModelToViewModel(TModel model)
        {
            return model == null ? null : _mapper.Map<TView>(model);
        }

        protected virtual TModel ViewModelToDbModel(TView view)
        {
            return view == null ? null : _mapper.Map<TModel>(view);
        }

        private IEnumerable<PropertyInfo> GetTypePropertiesInDeclarationOrder(Type type)
        {
            var thisLevel = type.GetProperties(BindingFlags.Public |
                                               BindingFlags.GetField |
                                               BindingFlags.SetField |
                                               BindingFlags.Instance |
                                               BindingFlags.DeclaredOnly)
                .Where(p => p.CanWrite);
            if (type.BaseType == null)
                return thisLevel;
            return GetTypePropertiesInDeclarationOrder(type.BaseType).Concat(thisLevel);
        }

        /// <summary>
        /// Return TableForm schema from class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldById"></param>
        /// <returns></returns>
        private TableForm ResolveFieldsFromType(Type type, Dictionary<string, TableFormField> fieldById)
        {
            // get properties of type, ignore some properties
            var dbFields = GetTypePropertiesInDeclarationOrder(type)
                .Select(field => new
                    {prop = field, attr = field.GetCustomAttributes<TableFormFieldAttribute>().FirstOrDefault()})
                .Where(propAndAttr => propAndAttr.attr == null || !propAndAttr.attr.Ignore)
                .ToList();
        
            return new TableForm
            {
                // map properties to fields
                Fields = dbFields
                    .OrderBy(propAndAttr => 0)
                    .Select(propAndAttr =>
                    {
                        var prop = propAndAttr.prop;
                        var attr = propAndAttr.attr;
                        // resolve field schema
                        var field = ResolveFieldByProperty(prop.Name, prop, prop.PropertyType, attr, fieldById);
                        fieldById[field.Id] = field;
                        return field;
                    })
                    .ToList(),
                Title = type.GetCustomAttributes<TableAttribute>().FirstOrDefault()?.Label ?? type.Name.Replace("View","") 
            };
        }

        /// <summary>
        /// Get type enum from C# type
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private TableFieldType ResolveFieldTypeFromType(Type propertyType)
        {
            // detect string
            if (propertyType == typeof(string))
                return TableFieldType.String;
            // detect any number type
            if (propertyType == typeof(int)
                || propertyType == typeof(uint)
                || propertyType == typeof(double)
                || propertyType == typeof(float)
                || propertyType == typeof(long)
                || propertyType == typeof(ulong)
                || propertyType == typeof(short)
                || propertyType == typeof(ushort)
            )
                return TableFieldType.Number;
            // detect boolean
            if (propertyType == typeof(bool))
                return TableFieldType.Boolean;
            // detect date
            if (propertyType == typeof(DateTime))
                return TableFieldType.Date;
            // throw unknown type field
            throw new UnsupportedFieldTypeException("TableFieldType not support this type property");
        }

        /// <summary>
        /// Turn code variable name to human readable name
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ToHumanReadable(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(str, " ");
        }

        // if nullable
        private bool IsTypeNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private (List<TableFieldChoice>, Type) ResolveEnumChoices(Type propPropertyType)
        {
            Type enumType = null;
            if (propPropertyType.IsEnum)
                enumType = propPropertyType;
            else if (IsTypeNullable(propPropertyType) &&
                     propPropertyType.GetGenericArguments()[0].IsEnum)
                enumType = propPropertyType.GetGenericArguments()[0];

            if (enumType == null) return (null, null);

            if (EnumAsString)
                return (Enum.GetNames(enumType).Select(name => new TableFieldChoice
                {
                    Id = name,
                    Label = ToHumanReadable(name)
                }).ToList(), enumType);

            return (Enum.GetValues(enumType).Cast<int>().Select(value => new TableFieldChoice
            {
                Id = value.ToString(),
                Label = ToHumanReadable(Enum.GetName(enumType, value))
            }).ToList(), enumType);
        }

        /// <summary>
        /// Generate field schema for property
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="member"></param>
        /// <param name="propPropertyType"></param>
        /// <param name="attr"></param>
        /// <param name="fieldById"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private TableFormField ResolveFieldByProperty(string propName,
            PropertyInfo member,
            Type propPropertyType,
            TableFormFieldAttribute attr,
            Dictionary<string, TableFormField> fieldById)
        {
            // get table field type
            var fieldType = ResolveFieldTypeFromType(propPropertyType);
            attr ??= new TableFormFieldAttribute();

            // basic property details
            var field = new TableFormField
            {
                ViewModelId = propName,
                DbModelId = propName,
                Id = propName,
                Type = fieldType,
                Label = attr?.Label ?? ToHumanReadable(propName)
            };

            if (attr?.Hidden ?? false)
                field.Hidden = true;

            // extend from data annotation attributes as well
            var requiredAttr = member.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null || (attr?.Required ?? false))
                field.Required = true;

            var readOnly = member.GetCustomAttribute<ReadOnlyAttribute>();
            if (readOnly?.IsReadOnly ?? attr?.ReadOnly ?? false)
                field.ReadOnly = true;

            // resolve choices for enums
            var (choices, enumType) = ResolveEnumChoices(propPropertyType);

            var hideChoicesAttr = member.GetCustomAttribute<TableIgnoreChoicesAttribute>();
            if (hideChoicesAttr != null)
                choices = choices.Where(c => hideChoicesAttr.IgnoreValues.All(v => v.ToString() != c.Id))
                    .ToList();
            field.Choices = choices;
            field.EnumType = enumType;

            return field;
        }
    }
}