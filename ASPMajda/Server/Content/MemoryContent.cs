using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Content
{
    class MemoryContent : IMemoryContent
    {
        public MemoryStream Stream { get; set;}
        public string MimeType { get; set; }

        public MemoryContent()
        {
            this.MimeType = "application/octet-stream";
            this.Stream = new MemoryStream();
        }

        public MemoryStream GetStream()
        {
            return this.Stream;
        }

        public string GetMime()
        {
            return this.MimeType;
        }
    }
}
