﻿using ASPMajda.Server.Content;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Messages
{
    class ResponseMessage
    {
        public static ResponseMessage Error = new StringResponseMessage(404, "<html><body><h1>Couldn't find endpoint...</h1></body></html>");

        public int StatusCode { get; set; }
        public Headers Headers { get; set; }
        public virtual IMemoryContent Content { get => null; set { value = null; } }

        public ResponseMessage(int statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new Headers();
        }
    }
}
