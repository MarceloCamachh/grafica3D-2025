using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;
using System;
namespace progGrafica1
{

    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            // Serializamos solo las propiedades X y Y
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(value.X);
            writer.WritePropertyName("Y");
            writer.WriteValue(value.Y);
            writer.WriteEndObject();
        }

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Lógica para leer un Vector2 desde el JSON (X y Y)
            JObject jo = JObject.Load(reader);
            return new Vector2((float)jo["X"], (float)jo["Y"]);
        }
    }

    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            // Serializamos solo las propiedades X, Y, Z
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(value.X);
            writer.WritePropertyName("Y");
            writer.WriteValue(value.Y);
            writer.WritePropertyName("Z");
            writer.WriteValue(value.Z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Lógica para leer un Vector3 desde el JSON (X, Y, Z)
            JObject jo = JObject.Load(reader);
            return new Vector3((float)jo["X"], (float)jo["Y"], (float)jo["Z"]);
        }
    }
}