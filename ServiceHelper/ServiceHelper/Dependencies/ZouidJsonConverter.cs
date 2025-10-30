using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    internal class ZouidJsonConverter : JsonConverter<Zouid>
    {
        public override Zouid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string @string = reader.GetString();
            if (!string.IsNullOrEmpty(@string))
            {
                return Zouid.Parse(@string);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, Zouid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
