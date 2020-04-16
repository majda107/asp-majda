using ASPMajda.Server.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Messages
{
    class FileContentMessage: ResponseMessage
    {
        public override IContent Content { get; set; }
        public FileContentMessage(int statusCode, string path):base(statusCode)
        {
            var fc = new FileContent(path);
            this.Content = fc;

            this.Headers.SetHeader("Content-Type", fc.MimeType); 
        }
    }
}
