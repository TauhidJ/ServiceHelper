using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    [JsonConverter(typeof(ZouidJsonConverter))]
    public class Zouid : BaseId
    {
        public bool IsRootOrganisationUnit => base.Value.IndexOf(':') == -1;

        public Zouid? ParentZouid
        {
            get
            {
                if (IsRootOrganisationUnit)
                {
                    return null;
                }

                int length = base.Value.LastIndexOf(':');
                return new Zouid(base.Value.Substring(0, length));
            }
        }

        public long OrganisationId
        {
            get
            {
                int num = base.Value.IndexOf(':');
                if (num == -1)
                {
                    return long.Parse(base.Value);
                }

                return long.Parse(base.Value.Substring(0, num));
            }
        }

        public int UnitId
        {
            get
            {
                int num = base.Value.LastIndexOf(':');
                if (num == -1)
                {
                    return 0;
                }

                string value = base.Value;
                int num2 = num + 1;
                return int.Parse(value.Substring(num2, value.Length - num2));
            }
        }

        public IEnumerable<int> GetAllUnitIds
        {
            get
            {
                yield return 0;
                int index = base.Value.IndexOf(':');
                string str = base.Value;
                while (index >= 0)
                {
                    string text = str;
                    int num = index + 1;
                    str = text.Substring(num, text.Length - num);
                    index = str.IndexOf(':');
                    if (index == -1)
                    {
                        yield return int.Parse(str);
                        break;
                    }

                    yield return int.Parse(str.Substring(0, index));
                }
            }
        }

        // ✅ EF Core requires a parameterless constructor
        private Zouid() : base(string.Empty)
        {
        }

        private Zouid(string value)
            : base(value)
        {
        }

        public static Zouid Parse(string value)
        {
            BaseId.EnsureValidNumericId(value);
            return new Zouid(value);
        }

        public static explicit operator Zouid(string value)
        {
            return Parse(value);
        }
    }
}
