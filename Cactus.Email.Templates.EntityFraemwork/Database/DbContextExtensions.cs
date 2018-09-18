using System;
using System.Reflection;
using Cactus.Email.Templates.EntityFraemwork.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Cactus.Email.Templates.EntityFraemwork.Database
{
    internal static class DbContextExtensions
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(DbContextExtensions));

        public static void Patch<T>(this DbContext context, JObject patch, T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Modified;
            foreach (var property in patch)
            {
                var propertyName = FirstLetterToUpper(property.Key);
                var currentProperty = typeof(T).GetProperty(propertyName);
                context.Entry(entity).Property(propertyName).IsModified = true;
                var jValue = property.Value.Value<JValue>();
                if (currentProperty.PropertyType.GetTypeInfo().IsEnum)
                {
                    SetValueForEnumProperty(jValue, currentProperty, entity);
                }
                else
                {
                    SetValueForProperty(jValue, currentProperty, entity);
                }
            }
        }

        private static void SetValueForProperty<T>(JValue jValue, PropertyInfo currentProperty, T entity)
        {
            switch (jValue.Type)
            {
                case JTokenType.None:
                    break;
                case JTokenType.Object:
                    break;
                case JTokenType.Array:
                    break;
                case JTokenType.Constructor:
                    break;
                case JTokenType.Property:
                    break;
                case JTokenType.Comment:
                    break;
                case JTokenType.Integer:
                    if (currentProperty.PropertyType == typeof(int))
                    {
                        currentProperty.SetValue(entity, jValue.ToObject<int>());
                    }
                    else if (currentProperty.PropertyType == typeof(double))
                    {
                        currentProperty.SetValue(entity, jValue.ToObject<double>());
                    }
                    else if (currentProperty.PropertyType == typeof(double?))
                    {
                        currentProperty.SetValue(entity, jValue.ToObject<double?>());
                    }
                    else if (currentProperty.PropertyType == typeof(int?))
                    {
                        currentProperty.SetValue(entity, jValue.ToObject<int?>());
                    }
                    break;
                case JTokenType.Float:
                    currentProperty.SetValue(entity,
                        currentProperty.PropertyType.GetTypeInfo().IsGenericType &&
                        currentProperty.PropertyType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>)
                            ? jValue.ToObject<double?>()
                            : jValue.ToObject<double>());
                    break;
                case JTokenType.String:
                    currentProperty.SetValue(entity, jValue.ToObject<string>());
                    break;
                case JTokenType.Boolean:
                    currentProperty.SetValue(entity, jValue.ToObject<bool>());
                    break;
                case JTokenType.Null:
                    currentProperty.SetValue(entity, null);
                    break;
                case JTokenType.Undefined:
                    break;
                case JTokenType.Date:
                    break;
                case JTokenType.Raw:
                    break;
                case JTokenType.Bytes:
                    break;
                case JTokenType.Guid:
                    break;
                case JTokenType.Uri:
                    break;
                case JTokenType.TimeSpan:
                    break;
                default:
                    var errorMessage = "Couldn't define type of property";
                    Logger.Error(errorMessage);
                    throw new ArgumentOutOfRangeException(nameof(currentProperty), errorMessage);
            }
        }

        private static void SetValueForEnumProperty<T>(JValue jValue, PropertyInfo currentProperty, T entity)
        {
            switch (jValue.Type)
            {
                case JTokenType.Integer:
                    currentProperty.SetValue(entity,
                        Convert.ChangeType(Enum.ToObject(currentProperty.PropertyType, jValue.ToObject<int>()),
                            currentProperty.PropertyType));
                    break;
                case JTokenType.String:
                    currentProperty.SetValue(entity,
                        Convert.ChangeType(Enum.Parse(currentProperty.PropertyType, jValue.ToObject<string>()),
                            currentProperty.PropertyType));
                    break;
            }
        }

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
