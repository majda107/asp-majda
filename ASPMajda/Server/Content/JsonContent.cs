using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Content
{
    class JsonContent: IMemoryContent
    {
        public string Json { get; private set; }
        public JsonContent(string json)
        {
            this.Json = json;
        }
        public MemoryStream GetStream()
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

        public string GetMime()
        {
            return "text/json";
        }
    }
}
