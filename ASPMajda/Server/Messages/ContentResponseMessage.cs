using ASPMajda.Server.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Messages
{
    class ContentResponseMessage: ResponseMessage
    {
        public override IContent Content { get; set; }

        public ContentResponseMessage(int statusCode, IContent content):base(statusCode)
        {
            this.Content = content;
        }
    }
}
