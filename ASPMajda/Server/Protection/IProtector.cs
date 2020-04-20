using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Protection
{
    interface IProtector
    {
        bool Allowed(RequestMessage request);
    }
}
