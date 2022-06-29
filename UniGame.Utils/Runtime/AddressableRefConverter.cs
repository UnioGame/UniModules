namespace Taktika.IAP.Runtime.Products
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using UnityEngine.AddressableAssets;

    public class AddressableRefConverter<T> : JsonConverter<T> where T:AssetReference
    {
        private const string GuidKey    = "guid";
        private const string SubNameKey = "subName";

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            var guid    = value.AssetGUID;
            var subName = value.SubObjectName;
            var obj     = new JObject();
            obj.Add(GuidKey, JToken.FromObject(guid));
            obj.Add(SubNameKey, JToken.FromObject(subName));
            obj.WriteTo(writer);
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj     = JObject.Load(reader);
            var guid    = obj[GuidKey].Value<string>();
            var subName = obj[SubNameKey].Value<string>();
            var result  = (T)Activator.CreateInstance(typeof(T), args: guid);
            result.SubObjectName = subName;
            return result;
        }
    }

}