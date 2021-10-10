using Newtonsoft.Json;
using System;
using System.Globalization;

namespace MyProject.Repository.Test.Helper
{
    public class DecimalJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((decimal)value).ToString("F2", CultureInfo.InvariantCulture));
        }
    }
    public class IntegratedUtils
    {
        public static T CloneList<T>(T data)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }

        public const string DatabaseTestDataPath = "Database/";
    }
}