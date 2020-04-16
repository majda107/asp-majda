using ASPMajda.Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Controller
{
    interface IControllerHandler
    {
        bool TryFire(RequestMessage request, out ResponseMessage response);
    }
}
