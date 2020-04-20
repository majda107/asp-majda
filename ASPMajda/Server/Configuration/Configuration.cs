using ASPMajda.App.Data;
using ASPMajda.Models;
using ASPMajda.Models.Result;
using ASPMajda.Server.Controller;
using ASPMajda.Server.Engine;
using ASPMajda.Server.Logger;
using ASPMajda.Server.Protection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ASPMajda.Server.Configuration
{
    class Configuration : IConfiguration
    {
        private List<Action<HttpServer>> mutations;
        public Configuration()
        {
            this.mutations = new List<Action<HttpServer>>();
        }
        public void AddDefaultMvc()
        {
            this.mutations.Add((server) =>
            {
                var mvc = new MVCControllerHandler<ControllerBase>(typeof(IResult), typeof(IResult).GetMethod("GetResponseMessage"));
                mvc.AddSingleton<MajdaService>(new MajdaService("Majda luf <3"));
                mvc.AddHttpClient();
                mvc.Generate();

                server.ServiceManager.ControllerHandlers.Add(mvc);
            });
        }

        public void ConfigureDefaultLogging()
        {
            this.mutations.Add(server =>
            {
                server.ServiceManager.Loggers.Add(new ConsoleLogger(Level.Detailed));
            });
        }

        public void AddControllerHandler(IControllerHandler controllerHandler)
        {
            this.mutations.Add((server) =>
            {
                server.ServiceManager.ControllerHandlers.Add(controllerHandler);
            });
        }

        public void ConfigureBlacklist(string[] blacklist)
        {
            this.mutations.Add((server) =>
            {
                var protector = new BlacklistProtector();
                foreach (var host in blacklist)
                    protector.Hosts.Add(host);

                server.ServiceManager.Protectors.Add(protector);
            });
        }
        public void ModifyServer(HttpServer server)
        {
            foreach (var mutation in this.mutations)
                mutation(server);
        }
    }
}
