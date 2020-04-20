using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace ASPMajda.Server.Content
{
    class FileContent: MemoryContentBase
    {

        public string FilePath { get; private set; }
        public FileContent(string path)
        {  
            this.FilePath = path;
        }

        public override MemoryStream GetStream()
        {
            var ms = new MemoryStream();
            using(var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(ms);
            }

            ms.Position = 0;
            return ms;
        }

        public override string GetMime()
        {
            var extension = Path.GetExtension(this.FilePath);
            if (MemoryContentBase.MimeMappings.ContainsKey(extension))
                return MemoryContentBase.MimeMappings[extension];
            else
                return "application/octet-stream";
        }

        public override object GetMvcResult(MethodInfo action, object controllerInstance)
        {
            if (!this.MvcMethodValid(action, typeof(MemoryStream))) return null;

            return action.Invoke(controllerInstance, new object[] { this.GetStream() });
        }
    }
}
