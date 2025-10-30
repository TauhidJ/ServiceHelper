using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    public class ZeidJsonConverter : JsonConverter<Zeid>
    {
        public override Zeid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string @string = reader.GetString();
            if (!string.IsNullOrEmpty(@string))
            {
                return Zeid.Parse(@string);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, Zeid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
