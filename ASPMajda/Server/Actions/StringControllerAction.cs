using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Actions
{
    class StringControllerAction : ControllerActionBase
    {
        public delegate ResponseMessage StringActionDelegate(string value);
        public StringActionDelegate Action { get; private set; }

        public StringControllerAction(Method method, string path, StringActionDelegate action) : base(method, path)
        {
            this.Action = action;
        }

        public override ResponseMessage Fire(MemoryContentBase content)
        {
            if (!(content is StringContent)) return ResponseMessage.Error;

            var stringContent = content as StringContent;

            if (this.Action != null)
                return this.Action(stringContent.Value);

            return ResponseMessage.Error;
        }
    }
}
