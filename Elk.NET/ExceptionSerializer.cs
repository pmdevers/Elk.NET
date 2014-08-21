using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Elk.NET
{
    public class ExceptionSerializer : IExceptionSerializer
    {
        public JObject Serialize(Exception exception)
        {
            var serialize = new JObject();

            var properties = exception.GetType().GetProperties();
            foreach (var property in properties)
            {
                AddProperty(property, serialize, exception);
            }

            return serialize;
        }

        private void AddProperty(System.Reflection.PropertyInfo property, JObject serialize, Exception exception)
        {
            var name = property.Name.ToLower();
            if (IsBulitinType(property.PropertyType))
            {
                try
                {
                    var value = property.GetValue(exception);
                    if (value != null)
                        serialize.Add(name, value.ToString());
                }
                catch (Exception ex)
                {
                   Debug.Write(ex.Message);
                }
            }
        }

        private static bool IsBulitinType(Type type)
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object);
        }
    }
}
