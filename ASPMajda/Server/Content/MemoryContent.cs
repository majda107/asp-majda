using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ASPMajda.Server.Content
{
    class MemoryContent : MemoryContentBase
    {
        public MemoryStream Stream { get; set;}
        public string MimeType { get; set; }

        public MemoryContent()
        {
            this.MimeType = "application/octet-stream";
            this.Stream = new MemoryStream();
        }

        public override MemoryStream GetStream()
        {
            return this.Stream;
        }

        public override string GetMime()
        {
            return this.MimeType;
        }

        public override object GetMvcResult(MethodInfo action, object controllerInstance)
        {
            if (!this.MvcMethodValid(action, typeof(MemoryStream))) return null;

            return action.Invoke(controllerInstance, new object[] { this.GetStream() });
        }
    }
}
