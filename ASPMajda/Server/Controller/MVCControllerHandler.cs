using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ASPMajda.Models;
using System.Linq;
using ASPMajda.Server.MVC;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Content;
using ASPMajda.Models.Result;
using ASPMajda.Models.Attributes;
using ASPMajda.Server.Packet;

namespace ASPMajda.Server.Controller
{
    class MVCControllerHandler<T> : IControllerHandler
    {
        public Type IResultType { get; private set; }
        public MethodInfo GetResponse { get; private set; }

        private Dictionary<string, ControllerData> controllerMethods;

        private Dictionary<Type, object> singletons;

        public MVCControllerHandler(Type iResultType, MethodInfo getResponse, bool init = false)
        {
            this.controllerMethods = new Dictionary<string, ControllerData>();
            this.singletons = new Dictionary<Type, object>();

            this.IResultType = iResultType;
            this.GetResponse = getResponse;

            if(init)
                this.Init();
        }

        public void Generate()
        {
            this.Init();
        }

        public void AddSingleton<S>(S singleton)
        {
            if (this.singletons.ContainsKey(typeof(S))) return;

            this.singletons.Add(typeof(S), singleton);
        }

        public void AddHttpClient()
        {
            if (this.singletons.ContainsKey(typeof(System.Net.Http.HttpClient))) return;
            this.singletons.Add(typeof(System.Net.Http.HttpClient), new System.Net.Http.HttpClient());
        }

        private IEnumerable<Type> GetChildren(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            return assembly.GetTypes().Where(t => t.IsSubclassOf(type));
        }

        private object[] GetSingletons(Type type)
        {
            int topIndex = 0;
            int topScore = 0;

            int score = 0;

            var constructors = type.GetConstructors();
            if (constructors.Length <= 0) return Array.Empty<object>();

            for (int i = 0; i < constructors.Length; i++)
            {
                score = 0;
                foreach (var parameter in constructors[i].GetParameters())
                    if (this.singletons.ContainsKey(parameter.ParameterType))
                        score += 1;

                if(score > topScore)
                {
                    topScore = score;
                    topIndex = i;
                }    
            }

            var instances = new List<object>();
            foreach(var parameter in constructors[topIndex].GetParameters())
            {
                if (singletons.ContainsKey(parameter.ParameterType))
                    instances.Add(singletons[parameter.ParameterType]);
                else
                    instances.Add(null);
            }

            return instances.ToArray();
        }
        private void Init()
        {
            var controllers = this.GetChildren(typeof(T));
            var results = this.GetChildren(this.IResultType);

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods();
                foreach (var method in methods)
                {
                    if (!results.Contains(method.ReturnType) && method.ReturnType != this.IResultType) continue;

                    var name = controller.Name.ToLower();
                    if (!this.controllerMethods.ContainsKey(name))
                    {
                        this.controllerMethods.Add(name, new ControllerData(controller, GetSingletons(controller)));
                    }

                    this.controllerMethods[name].Add(method);
                }
            }
        }

        public bool TryFire(RequestMessage request, out ResponseMessage response)
        {
            response = ResponseMessage.Error;

            string[] pathSplit = request.Path.Split('/');
            if (pathSplit.Length < 3) return false;

            if (!this.controllerMethods.ContainsKey(pathSplit[1])) return false;

            var data = this.controllerMethods[pathSplit[1]];
            var action = data.Methods.First(m => m.Name.ToLower() == pathSplit[2]);

            var methodAttr = action.GetCustomAttribute<FromMethod>();
            var method = methodAttr == null ? Method.GET : methodAttr.Method;

            if (action == null || method != request.Method) return false;

            // YOU COULD RETURN NULL IN CONTROLLER AND FAKE TRYFIRE!
            object result = null;

            var parameters = action.GetParameters();
            if(parameters.Length > 1 || parameters.Length == 0)
            {
                var objects = new List<object>();
                foreach (var parameter in parameters)
                    objects.Add(null);

                result = (action.Invoke(data.ControllerInstance, objects.ToArray()));
            }


            if (request.Body is JsonContent && action.GetCustomAttribute<FromJson>() != null)
            {
                var param = (request.Body as JsonContent).GetObject(action.GetParameters().First().ParameterType);
                result = (action.Invoke(data.ControllerInstance, new object[] { param }));
            }

            if (request.Body is StringContent && action.GetParameters().First().ParameterType == typeof(string))
            {
                result = (action.Invoke(data.ControllerInstance, new object[] { (request.Body as StringContent).Value }));
            }



            if (result != null)
                response = (ResponseMessage)this.GetResponse.Invoke(result, new object[] { });

            return true;
        }
    }
}
