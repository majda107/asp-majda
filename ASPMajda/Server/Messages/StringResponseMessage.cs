using ASPMajda.Server.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Messages
{
    class StringResponseMessage: ResponseMessage
    {
        public override IMemoryContent Content { get; set; }

        public StringResponseMessage(int statusCode, string message) :base(statusCode)
        {
            this.Headers.SetHeader("Content-Type", "text/html; charset=utf-8");
            this.Content = new StringContent(message);
        }
    }
}
