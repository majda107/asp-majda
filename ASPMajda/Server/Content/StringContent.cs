using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ASPMajda.Server.Content
{
    class StringContent : MemoryContentBase
    {
        public string Value { get; set; }
        public StringContent(string value)
        {
            this.Value = value;
        }
        public override MemoryStream GetStream()
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(this.Value);
            sw.Flush();
            ms.Position = 0;
            return ms;
        }

        public override string GetMime()
        {
            return "text/plain";
        }

        public override object GetMvcResult(MethodInfo action, object controllerInstance)
        {
            if (!this.MvcMethodValid(action, typeof(string))) return null;

            return action.Invoke(controllerInstance, new object[] { this.Value });
        }
    }
}
