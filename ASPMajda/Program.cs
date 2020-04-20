using ASPMajda.App.Models;
using ASPMajda.Server;
using ASPMajda.Server.Actions;
using ASPMajda.Server.Configuration;
using ASPMajda.Server.Content;
using ASPMajda.Server.Controller;
using ASPMajda.Server.Engine;
using ASPMajda.Server.Messages;
using ASPMajda.Server.Packet;
using System;
using System.IO;
using System.Net;

namespace ASPMajda
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer(IPAddress.Any, 6969);

            Console.WriteLine($"Starting server at port {server.Port}");

            var configuration = new Configuration();

            // configure default mvc
            configuration.AddDefaultMvc();
            // configure logger
            configuration.ConfigureDefaultLogging();
            // configure blacklist
            configuration.ConfigureBlacklist(new string[]{ "127.0.0.1:6969" });


            // aditional endpoints
            var custom = new ControllerHandler();
            custom.Register(new ControllerAction(Method.GET, "/test", (body) =>
            {
                Console.WriteLine("HAHAHA WORKS!");
                return ResponseMessage.Error;
            }));

            custom.Register(new ControllerAction(Method.POST, "/testpost", (body) =>
            {
                Console.WriteLine("POST WORKING!");

                if (body != null)
                    Console.WriteLine($"Received body type: {body.GetMime()}");

                if (body is StringContent)
                    Console.WriteLine(((body as StringContent).Value));

                return ResponseMessage.Error;
            }));

            custom.Register(new JsonControllerAction<ArticleViewModel>(Method.POST, "/postarticle", (article) =>
            {
                Console.WriteLine("Article seems to be received!");
                Console.WriteLine($"Text: {article.Text}");

                return ResponseMessage.Error;
            }));


            configuration.AddControllerHandler(custom);
            configuration.AddControllerHandler(new PublicFolderControllerHandler("/_public", Path.Combine(Directory.GetCurrentDirectory())));
            configuration.AddControllerHandler(new FileControllerHandler(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            server.Configure(configuration);

            server.Listen();
        }
    }
}
