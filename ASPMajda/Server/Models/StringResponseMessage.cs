using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server.Models
{
    class StringResponseMessage: ResponseMessage
    {
        public override IContent Content { get; set; }

        public StringResponseMessage(int statusCode, string message) :base(statusCode)
        {
            this.Headers.Add("Content-Type", "text/html; charset=utf-8");
            this.Content = new StringContent(message);
        }
    }
}
