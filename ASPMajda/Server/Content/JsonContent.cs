using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ASPMajda.Server.Content
{
    class JsonContent: MemoryContentBase
    {
        public string Json { get; private set; }
        public JsonContent(string json)
        {
            this.Json = json;
        }
        public override MemoryStream GetStream()
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(this.Json);
            sw.Flush();
            ms.Position = 0;
            return ms;
        }

        public T GetObject<T>()
        {
            //var jobject = JObject.Parse(this.Json);
            //JsonConvert.DeserializeObject
            //if(jobject.toobj)
            return JsonConvert.DeserializeObject<T>(this.Json);
        }

        public object GetObject(Type type)
        {   
            try
            {
                return JsonConvert.DeserializeObject(this.Json, type);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public override string GetMime()
        {
            return "text/json";
        }

        public override object GetMvcResult(MethodInfo action, object controllerInstance)
        {
            var args = action.GetParameters();
            if (args.Length <= 0) return null;
            if (args[0].ParameterType == typeof(string)) return null;

            var param = this.GetObject(action.GetParameters().First().ParameterType);
            return action.Invoke(controllerInstance, new object[] { param });
        }
    }
}
