using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Elk.NET
{
    public class ExceptionSerializer
    {
        public string SerializeObject(Exception exception, DateTime dateTime)
        {
            var serialize = new JObject { { "timestamp", dateTime.ToString("o") } };

            var properties = exception.GetType().GetProperties();
            foreach (var property in properties)
            {
                AddProperty(property, serialize, exception);
            }

            return serialize.ToString();
        }

        private void AddProperty(System.Reflection.PropertyInfo property, JObject serialize, Exception exception)
        {
            var name = property.Name.ToLower();
            if (IsBulitinType(property.PropertyType))
            {
                var value = property.GetValue(exception);

                if (value != null)
                    serialize.Add(name, value.ToString());
            }
        }

        private static bool IsBulitinType(Type type)
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object);
        }
    }
}
