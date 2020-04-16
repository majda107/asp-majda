using ASPMajda.Server.Controller;
using ASPMajda.Server.Logger;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using ASPMajda.Server.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASPMajda.Server
{
    class ServiceManager
    {
        public IList<IControllerHandler> ControllerHandlers { get; private set; }
        public IList<ILogger> Loggers { get; private set; }

        public ServiceManager()
        {
            this.ControllerHandlers = new List<IControllerHandler>();
            this.Loggers = new List<ILogger>();

            // TEST
            this.Init();
            this.InitLogging();
        }

        private void Init()
        {
            var custom = new ControllerHandler();
            custom.Register(new ControllerAction(Method.GET, "/test", (body) =>
            {
                Console.WriteLine("HAHAHA WORKS!");
                return ResponseMessage.Error;
            }));

            custom.Register(new ControllerAction(Method.POST, "/testpost", (body) =>
            {
                Console.WriteLine("POST WORKING!");
                if(body is StringContent)
                    Console.WriteLine((body as StringContent).Value);

                return ResponseMessage.Error;
            }));

            this.ControllerHandlers.Add(new FileControllerHandler(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            this.ControllerHandlers.Add(custom);
        }

        private void InitLogging()
        {
            this.Loggers.Add(new ConsoleLogger());
        }

        public void HandleLog(string message, Level level)
        {
            foreach (var logger in this.Loggers)
                logger.Log(message, level);
        }


        public void HandleControllers(RequestMessage request, out ResponseMessage response)
        {
            this.HandleLog("Handling controllers", Level.Detailed);

            response = ResponseMessage.Error;

            foreach (var handler in this.ControllerHandlers)
                if (handler.TryFire(request, out response))
                    this.HandleLog("Handler found!", Level.Info);
        }
    }
}
