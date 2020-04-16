using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace ASPMajda.Server.Content
{
    class FileContent: IMemoryContent
    {

        public string FilePath { get; private set; }
        public FileContent(string path)
        {  
            this.FilePath = path;
        }

        public MemoryStream GetStream()
        {
            var ms = new MemoryStream();
            using(var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(ms);
            }

            ms.Position = 0;
            return ms;
        }

        public string GetMime()
        {
            var extension = Path.GetExtension(this.FilePath);
            if (IMemoryContent.MimeMappings.ContainsKey(extension))
                return IMemoryContent.MimeMappings[extension];
            else
                return "application/octet-stream";
        }
    }
}
