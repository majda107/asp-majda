using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ASPMajda.Server.MVC
{
    class ControllerData
    {
        public List<MethodInfo> Methods { get; set; }
        public object ControllerInstance { get; set; }

        public ControllerData(Type type, params object[] args)
        {
            this.Methods = new List<MethodInfo>();
            this.ControllerInstance = Activator.CreateInstance(type, args);
        }

        public void Add(MethodInfo method)
        {
            this.Methods.Add(method);
        }
    }
}
