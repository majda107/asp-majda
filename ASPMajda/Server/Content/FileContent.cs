using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace ASPMajda.Server.Content
{
    class FileContent: IContent
    {

        public string MimeType { get; private set; }
        public string FilePath { get; private set; }
        public FileContent(string path)
        {
            var extension = Path.GetExtension(path);
            if (IContent.MimeMappings.ContainsKey(extension))
                this.MimeType = IContent.MimeMappings[extension];
            else
                this.MimeType = "text/plain";

            this.FilePath = path;
        }

        public Stream GetStream()
        {
            var ms = new MemoryStream();
            using(var fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(ms);
            }

            ms.Position = 0;
            return ms;
        }
    }
}
