using Newtonsoft.Json;

namespace Structura.SharedComponents.Utilities
{
    public class ObjectToString
    {
        public string GetObjectFields(object o)
        {
            return JsonConvert.SerializeObject(o, 
                Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }
    }
}
