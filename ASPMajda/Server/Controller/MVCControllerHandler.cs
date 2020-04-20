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
using System.IO;

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

            if (init)
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

                if (score > topScore)
                {
                    topScore = score;
                    topIndex = i;
                }
            }

            var instances = new List<object>();
            foreach (var parameter in constructors[topIndex].GetParameters())
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

        private MethodInfo GetBodyAction(string name, ControllerData data, MemoryContentBase content, Method method)
        {
            var candidates = data.Methods.Where(m => m.Name.ToLower() == name.ToLower());
            if (method != Method.GET)
                candidates = candidates.Where(t =>
                {
                    var att = t.GetCustomAttribute<FromMethod>();
                    if (att == null) return false;
                    return att.Method == method;
                });

            MethodInfo noParam = null;
            foreach (var candidate in candidates)
            {
                var parameters = candidate.GetParameters();
                if (parameters.Length <= 0)
                    noParam = candidate;

                if (parameters.Length > 1) continue;

                var attribute = candidate.GetCustomAttribute<FromJson>();
                if (content is JsonContent && attribute != null)
                    return candidate;

                if (content is StringContent && parameters[0].ParameterType == typeof(string))
                    return candidate;

                if (content is FormContent && parameters[0].ParameterType == typeof(FormContent))
                    return candidate;
            }

            if (noParam != null)
                return noParam;

            return null;
        }

        public bool TryFire(RequestMessage request, out ResponseMessage response)
        {
            response = ResponseMessage.Error;
            if (request.Path == null) return false;

            string[] pathSplit = request.Path.Split('/');
            if (pathSplit.Length < 3) return false;

            if (!this.controllerMethods.ContainsKey(pathSplit[1])) return false;

            var data = this.controllerMethods[pathSplit[1]];
            var actions = data.Methods.Where(m => m.Name.ToLower() == pathSplit[2]);
            //var action = data.Methods.First(m => m.Name.ToLower() == pathSplit[2]);
            //var action = this.GetBodyAction(pathSplit[2], data, request.Body, request.Method);

            //if (action == null) return false;



            //// YOU COULD RETURN NULL IN CONTROLLER AND FAKE TRYFIRE!
            object result = null;

            //var parameters = action.GetParameters();
            //if (parameters.Length > 1 || parameters.Length == 0)
            //{
            //    var objects = new List<object>();
            //    foreach (var parameter in parameters)
            //        objects.Add(null);

            //    result = (action.Invoke(data.ControllerInstance, objects.ToArray()));
            //}


            //if (request.Body is JsonContent && action.GetCustomAttribute<FromJson>() != null)
            //{
            //    var param = (request.Body as JsonContent).GetObject(action.GetParameters().First().ParameterType);
            //    result = (action.Invoke(data.ControllerInstance, new object[] { param }));
            //}

            //if (request.Body is StringContent && action.GetParameters().First().ParameterType == typeof(string))
            //{
            //    result = (action.Invoke(data.ControllerInstance, new object[] { (request.Body as StringContent).Value }));
            //}

            //if (request.Body is FormContent && action.GetParameters().First().ParameterType == typeof(FormContent))
            //    result = (action.Invoke(data.ControllerInstance, new object[] { (request.Body as FormContent) }));


            if(request.Body == null)
            {
                if (actions.Count() <= 0) return false;
                var action = actions.OrderBy(t => t.GetParameters().Length).First();

                var objects = new List<object>();
                foreach (var parameter in action.GetParameters())
                    objects.Add(null);

                result = (action.Invoke(data.ControllerInstance, objects.ToArray()));

                response = (ResponseMessage)this.GetResponse.Invoke(result, new object[] { });
                return true;
            }

            foreach (var action in actions)
            {
                result = request.Body.GetMvcResult(action, data.ControllerInstance);
                if (result != null)
                {
                    response = (ResponseMessage)this.GetResponse.Invoke(result, new object[] { });
                    return true;
                }
            }
                

            //if (result != null)
            //    response = (ResponseMessage)this.GetResponse.Invoke(result, new object[] { });

            return false;
        }
    }
}
