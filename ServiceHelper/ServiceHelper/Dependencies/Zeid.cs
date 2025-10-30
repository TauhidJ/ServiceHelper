using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    [JsonConverter(typeof(ZeidJsonConverter))]
    public class Zeid : BaseId
    {
        public EntityType EntityType { get; }

        public IEnumerable<object> GetIds
        {
            get
            {
                string value = base.Value;
                int length = EntityType.Value.Length;
                string[] array = value.Substring(length, value.Length - length).Split(':', StringSplitOptions.RemoveEmptyEntries);
                string[] array2 = array;
                foreach (string text in array2)
                {
                    Guid result2;
                    if (long.TryParse(text, out var result))
                    {
                        yield return result;
                    }
                    else if (Guid.TryParse(text, out result2))
                    {
                        yield return result2;
                    }
                }
            }
        }

        private Zeid(EntityType entityType, string value)
            : base(value)
        {
            EntityType = entityType;
        }

        public static Zeid Create(EntityType entityType, List<long> ids)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            if (ids == null || ids.Count == 0)
            {
                throw new ArgumentNullException("ids");
            }

            string value = entityType.Value + ":" + string.Join(':', ids.Select((long m) => m.ToString()));
            return new Zeid(entityType, value);
        }

        public static Zeid Create(EntityType entityType, Guid id)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            string value = entityType.Value;
            Guid guid = id;
            string value2 = value + ":" + guid;
            return new Zeid(entityType, value2);
        }

        public static Zeid Create(EntityType entityType, IEnumerable<object> ids)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids");
            }

            IEnumerable<string> values = ids.Select(delegate (object id)
            {
                if (id is long num)
                {
                    return num.ToString();
                }

                if (id is int num2)
                {
                    return num2.ToString();
                }

                if (!(id is Guid guid))
                {
                    throw new ArgumentException("Unsupported ID type '" + id.GetType().Name + "'. Only long, int, and Guid are allowed.");
                }

                return guid.ToString();
            });
            string value = entityType.Value + ":" + string.Join(':', values);
            return new Zeid(entityType, value);
        }

        public static Zeid Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            if (!Regex.IsMatch(value, "^[a-z]+(:[a-z]+){1,}:[^:]+:[^:]+$$"))
            {
                throw new ArgumentException("Invalid format of the value.", "value");
            }

            EntityType? entityType = GetEntityType(value);
            if (GetEntityType(value) == null)
            {
                throw new InvalidCastException("Value does not have valid entity type.");
            }

            return new Zeid(entityType, value);
        }

        private static EntityType? GetEntityType(string value)
        {
            EntityType entityType = null;
            foreach (EntityType item in EntityType.List)
            {
                if (value.StartsWith(item.Value, StringComparison.Ordinal) && (entityType == null || item.Value.Length > entityType.Value.Length))
                {
                    entityType = item;
                }
            }

            return entityType;
        }

        public static explicit operator Zeid(string value)
        {
            return Parse(value);
        }
    }
}
