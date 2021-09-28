using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BFF.Models.Attributes;
using BFF.Models.Interfaces;
using BFF.Service.Exceptions;
using BFF.Service.Extensions;
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
        private readonly Dictionary<string, TableFormField> _fieldById = new Dictionary<string, TableFormField>();
        private readonly Dictionary<string, TableColumnField> _columnById = new Dictionary<string, TableColumnField>();
        private Dictionary<string, FormScheme> _dialogs = new Dictionary<string, FormScheme>();

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

        public bool EnumAsString { get; set; } = true;


        [HttpGet("list")]
        public virtual async Task<List<TView>> GetListAsync(CancellationToken ct)
        {
            var collection = _dal.GetCollection<TModel>();
            var models = await collection.Find(FilterDefinition<TModel>.Empty).ToListAsync(ct);
            return models.Select(DbModelToViewModel).ToList();
        }

        [HttpGet("form")]
        public FormScheme GetTableFormAsync(CancellationToken ct)
        {
            var form = ResolveFormScheme(typeof(TView));
            return form;
        }
        
        [HttpGet("columns")]
        [AllowAnonymous]
        public List<TableColumnField> GetTableColumnsAsync(CancellationToken ct)
        {
            var viewType = typeof(TView);
            var columns = ResolveTableColumns(viewType, out var defaultSort)
                .OrderBy((columnAndOrder) => columnAndOrder.Item2)
                .Select((columnAndOrder) => columnAndOrder.Item1)
                .ToList();
            return columns;
        }

        [HttpGet("actions")]
        [AllowAnonymous]
        public List<TableAction> GetTableActionsAsync(CancellationToken ct)
        {
            var actions = ResolveControllerActions();
            return actions;
        }
        
        [HttpGet("table")]
        [AllowAnonymous]
        public TableScheme GetTableSchemeAsync(CancellationToken ct)
        {
            var viewType = typeof(TView);
            
            var controllerType = GetType();
            var tableAttr = viewType.GetCustomAttributes<TableAttribute>().FirstOrDefault();

            var table = new TableScheme()
            {
                FormScheme = ResolveFormScheme(viewType),
                Columns = ResolveTableColumns(viewType, out var defaultSort)
                    .OrderBy((columnAndOrder) => columnAndOrder.Item2)
                    .Select((columnAndOrder) => columnAndOrder.Item1)
                    .ToList(),
                Actions = ResolveControllerActions(),
                Id = controllerType.Name.Replace("Controller", ""),
                Label = tableAttr?.Label ?? ToHumanReadable(viewType.Name),
                ColumnsById = _columnById,
                FieldsById = _fieldById,
                PaginationLimit = tableAttr?.PaginationLimit,
                IsOrderable = false, // TODO - implement the order & sort logic
                DefaultSort = defaultSort,
            };
            if (tableAttr?.NoDelete ?? false)
                table.NoDelete = true;
            if (tableAttr?.NoCreate ?? false)
                table.NoCreate = true;
            if (tableAttr?.NoEdit ?? false)
                table.NoEdit = true;
            var tags = controllerType.GetCustomAttributes<TagAttribute>()
                .SelectMany(t => t.Tags).Distinct().ToList();
            if (tags.Count > 0)
            {
                table.Tags = tags;
            }

            return table;
        }
        

        
        private List<TableAction> ResolveControllerActions()
        {
            var controllerActions = GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Select((method) => new {method = method, attr = method.GetCustomAttributes<TableActionAttribute>().FirstOrDefault()})
                .Where((methodAndAttr) => methodAndAttr.attr != null)
                .ToList();
            return controllerActions.Select((methodAndAttr) =>
            {
                var actionMethod = methodAndAttr.method;
                Debug.Assert(actionMethod.ReturnType == typeof(TableActionResult) ||
                             actionMethod.ReturnType == typeof(Task<TableActionResult>),
                    "Action must return TableActionResult");
                var actionAttr = methodAndAttr.attr;
                // get dialog parameter
                var actionParam = actionMethod.GetParameters()
                    .FirstOrDefault(p =>
                        typeof(TableActionRequest).IsAssignableFrom(p.ParameterType) && p.GetCustomAttributesData()
                            .FirstOrDefault(a =>
                                a.AttributeType == typeof(FromBodyAttribute) ||
                                a.AttributeType == typeof(FromFormAttribute)) != null);
                Debug.Assert(actionParam != null, "No action body found");
                var actionTags = actionMethod.GetCustomAttributes<TagAttribute>()
                    .SelectMany(t => t.Tags).Distinct().ToList();
                return new TableAction
                {
                    // id of action
                    Id = actionMethod.Name.Replace("Async", ""),
                    Label = actionAttr.Label ?? ToHumanReadable(actionMethod.Name.Replace("Async", "")),
                    IsMulti = actionAttr.IsMulti,
                    Filter = actionAttr.Filter,
                    Dialog = actionParam.ParameterType.GenericTypeArguments.Length == 1
                        ? RegisterDialog(actionParam.ParameterType.GenericTypeArguments[0])
                        : null,
                    IsGlobal = actionAttr.IsGlobal,
                    Tags = actionTags.Count > 0 ? actionTags : null,
                    ShowInTable = actionAttr.ShowInTable,
                    ShowInForm = actionAttr.ShowInView,
                    NoQuery = actionAttr.NoQuery
                };
            }).ToList();
        }

        private string RegisterDialog(Type dialogModel)
        {
            string dialogName = dialogModel.Name;
            if (!_dialogs.ContainsKey(dialogName))
            {
                _dialogs[dialogName] = ResolveFormScheme(dialogModel);
            }
            return dialogName;
        }

        protected virtual TView DbModelToViewModel(TModel model)
        {
            return model == null ? null : _mapper.Map<TView>(model);
        }

        protected virtual TModel ViewModelToDbModel(TView view)
        {
            return view == null ? null : _mapper.Map<TModel>(view);
        }
        
        /// <summary>
        /// Return Table Form schema from class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private FormScheme ResolveFormScheme(Type type)
        {
            // get properties of type & ignore some properties
            var dbFields = GetTypePropertiesInDeclarationOrder(type)
                .Select(field => new {prop = field, attr = field.GetCustomAttributes<TableFormFieldAttribute>().FirstOrDefault()})
                .Where(propAndAttr => propAndAttr.attr == null || !propAndAttr.attr.Ignore)
                .ToList();

            return new FormScheme
            {
                // map properties to fields
                Fields = dbFields
                    .OrderBy(propAndAttr => 0)
                    .Select(propAndAttr =>
                    {
                        var prop = propAndAttr.prop;
                        var attr = propAndAttr.attr;
                        // resolve field schema
                        var field = ResolveFieldByProperty(prop.Name, prop, prop.PropertyType, attr);
                        _fieldById[field.Id] = field;
                        return field;
                    })
                    .ToList(),
                Title = type.GetCustomAttributes<TableAttribute>().FirstOrDefault()?.Label ??
                        type.Name.Replace("View", "")
            };
        }

        private List<(TableColumnField, int)> ResolveTableColumns(Type tableType,
            out SortItem defaultSort)
        {
            // get class properties, that has attribute on them
            var dbFields = tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Select(field => new
                        {field, attr = field.GetCustomAttributes<TableColumnAttribute>().FirstOrDefault()})
                    .Where(fieldAndAttr => fieldAndAttr.attr != null)
                    .ToList();
            SortItem foundDefaultSort = null;
            var columns = dbFields
                // order fields
                .SelectMany(fieldAndAttr =>
                {
                    var property = fieldAndAttr.field;
                    var id = property.Name;
                    var field = _fieldById.GetOrDefault(id);
                    var attr = fieldAndAttr.attr;
                    if (attr.IsDefaultSort)
                        foundDefaultSort = new SortItem
                        {
                            Field = id,
                            IsDesc = attr.DefaultSortDesc
                        };

                    var type = ResolveFieldTypeFromType(property.PropertyType);
                    var (choices, enumType) = ResolveEnumChoices(property.PropertyType);

                    var column = new TableColumnField()
                    {
                        Id = id,
                        Label = attr?.Label ?? ToHumanReadable(property.Name),
                        Sortable = !(
                            // if explicit no sortable
                            attr?.IsNotSortable ??
                            // is type sortable
                            IsNotSortableType(type)),
                        Operators = (attr?.Operators ?? GetOperatorsPerType(type, choices != null))
                            .ToList(),
                        Type = type,
                        Formatter = attr?.Formatter
                    };
                    _columnById[id] = column;
                    return new List<(TableColumnField, int)> {(column, attr?.Ordinal ?? 0)};
                }).ToList();
            defaultSort = foundDefaultSort;
            if (defaultSort == null)
                defaultSort = new SortItem
                {
                    Field = "ordinal",
                    IsDesc = false
                };
            return columns;
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

        
        private static TableOperator[] GetOperatorsPerType(TableFieldType type,
            bool hasChoices)
        {
            if (hasChoices)
            {
                return new[] {TableOperator.In};
            }

            return type switch
            {
                TableFieldType.Number => new[]
                {
                    TableOperator.Equal, TableOperator.NotEqual, TableOperator.Gt, TableOperator.Gte,
                    TableOperator.Lt, TableOperator.Lte
                },
                TableFieldType.Date => new[]
                {
                    TableOperator.Equal, TableOperator.Gte, TableOperator.Lte, TableOperator.Range
                },
                TableFieldType.String => new[] {TableOperator.Like},
                TableFieldType.Boolean => new[] {TableOperator.Equal},
                _ => new[] {TableOperator.Equal, TableOperator.NotEqual}
            };
        }
        
        private bool IsNotSortableType(TableFieldType type)
        {
            switch (type)
            {
                case TableFieldType.String:
                case TableFieldType.Number:
                case TableFieldType.Boolean:
                case TableFieldType.Date:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        /// <summary>
        ///     Get type enum from C# type
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


        private static string ToHumanReadable(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(str, " ");
        }

        private static bool IsTypeNullable(Type type)
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
        ///     Generate field schema for property
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="member"></param>
        /// <param name="propPropertyType"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private TableFormField ResolveFieldByProperty(string propName,
            PropertyInfo member,
            Type propPropertyType,
            TableFormFieldAttribute attr)
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