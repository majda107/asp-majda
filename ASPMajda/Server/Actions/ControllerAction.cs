using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Actions
{
    class ControllerAction: ControllerActionBase
    {
        public delegate ResponseMessage ControllerActionDelegate(IMemoryContent body);
        public ControllerActionDelegate Action { get; private set; }


        public ControllerAction(Method method, string path, ControllerActionDelegate action):base(method, path)
        {
            this.Action = action;
        }

        public override ResponseMessage Fire(IMemoryContent body = null)
        {
            if (this.Action == null) return null;

            // wrap to return response message
            return this.Action(body);
        }
    }
}
