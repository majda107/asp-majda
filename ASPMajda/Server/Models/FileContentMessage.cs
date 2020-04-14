using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Models
{
    class FileContentMessage: ResponseMessage
    {
        public override IContent Content { get; set; }
        public FileContentMessage(int statusCode, string path):base(statusCode)
        {
            var fc = new FileContent(path);
            this.Content = fc;

            if (!this.Headers.ContainsKey("Content-Type")) this.Headers.Add("Content-Type", "");
            this.Headers["Content-Type"] = fc.MimeType;
        }
    }
}
