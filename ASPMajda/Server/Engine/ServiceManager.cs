using ASPMajda.Server.Controller;
using ASPMajda.Server.Logger;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using ASPMajda.Server.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ASPMajda.Server.Actions;
using ASPMajda.App.Models;
using ASPMajda.Models;
using ASPMajda.Models.Result;
using ASPMajda.App.Data;
using ASPMajda.Server.Protection;

namespace ASPMajda.Server.Engine
{
    class ServiceManager
    {
        public IList<IControllerHandler> ControllerHandlers { get; private set; }
        public IList<ILogger> Loggers { get; private set; }
        public IList<IProtector> Protectors { get; private set; }

        public ServiceManager()
        {
            this.ControllerHandlers = new List<IControllerHandler>();
            this.Loggers = new List<ILogger>();
            this.Protectors = new List<IProtector>();

            // TEST
            //this.Init();
            //this.InitLogging();
        }

        //private void Init()
        //{
        //    var custom = new ControllerHandler();
        //    custom.Register(new ControllerAction(Method.GET, "/test", (body) =>
        //    {
        //        Console.WriteLine("HAHAHA WORKS!");
        //        return ResponseMessage.Error;
        //    }));

        //    custom.Register(new ControllerAction(Method.POST, "/testpost", (body) =>
        //    {
        //        Console.WriteLine("POST WORKING!");

        //        if(body != null)
        //            Console.WriteLine($"Received body type: {body.GetMime()}");

        //        if(body is StringContent)
        //            Console.WriteLine((body as StringContent).Value);

        //        return ResponseMessage.Error;
        //    }));

        //    custom.Register(new JsonControllerAction<ArticleViewModel>(Method.POST, "/postarticle", (article) =>
        //    {
        //        Console.WriteLine("Article seems to be received!");
        //        Console.WriteLine($"Text: {article.Text}");

        //        return ResponseMessage.Error;
        //    }));

        //    //var mvc = new MVCControllerHandler<ControllerBase>(typeof(IResult), typeof(IResult).GetMethod("GetResponseMessage"));
        //    //mvc.AddSingleton<MajdaService>(new MajdaService("Majda luf <3"));
        //    //mvc.AddHttpClient();
        //    //mvc.Generate();
        //    //this.ControllerHandlers.Add(mvc);

        //    this.ControllerHandlers.Add(custom);
        //    this.ControllerHandlers.Add(new FileControllerHandler(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        //}

        //private void InitLogging()
        //{
        //    this.Loggers.Add(new ConsoleLogger());
        //}

        public void HandleLog(string message, Level level)
        {
            foreach (var logger in this.Loggers)
                lock (this)
                    logger.Log(message, level);
        }

        public void HandleWarning(string message) { foreach (var logger in this.Loggers) lock (this) { logger.Warn(message); } }
        public void HandleError(string message) { foreach (var logger in this.Loggers) lock (this) { logger.Error(message); } }
        public void HandleOk(string message) { foreach (var logger in this.Loggers) lock (this) { logger.Ok(message); } }

        public void HandleResponseError() => HandleError("Error while sending HTTP response...");
        public void HandleRequestError() => HandleError("Error while receiving HTTP request...");


        public void HandleControllers(RequestMessage request, out ResponseMessage response)
        {
            this.HandleLog("Handling controllers", Level.Detailed);

            response = ResponseMessage.Error;

            foreach (var handler in this.ControllerHandlers)
                if (handler.TryFire(request, out response))
                {
                    this.HandleLog("Handler found!", Level.Info);
                    return;
                }
        }

        public bool HandleProtectors(RequestMessage request)
        {
            foreach (var protector in this.Protectors)
                if (!protector.Allowed(request)) return false;

            return true;
        }
    }
}
