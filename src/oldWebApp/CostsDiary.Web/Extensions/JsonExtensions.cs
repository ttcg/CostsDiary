using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CostsDiary.Web.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, setting);
        }
    }
}
