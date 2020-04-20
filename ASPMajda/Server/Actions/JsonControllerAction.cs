using ASPMajda.Server.Content;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.Server.Actions
{
    class JsonControllerAction<T> : ControllerActionBase
    {
        public delegate ResponseMessage JsonActionDelegate(T value);
        public JsonActionDelegate Action { get; private set; }

        public JsonControllerAction(Method method, string path, JsonActionDelegate action) : base(method, path)
        {
            this.Action = action;
        }

        public override ResponseMessage Fire(MemoryContentBase content)
        {
            if (!(content is JsonContent)) return ResponseMessage.Error;

            var jsonContent = content as JsonContent;

            if (this.Action != null)
                return this.Action(jsonContent.GetObject<T>());

            return ResponseMessage.Error;
        }
    }
}
