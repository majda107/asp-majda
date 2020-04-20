using ASPMajda.App.Data;
using ASPMajda.App.Models;
using ASPMajda.Models;
using ASPMajda.Models.Attributes;
using ASPMajda.Models.Result;
using ASPMajda.Server.Content;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ASPMajda.App.Controllers
{
    class HomeController: ControllerBase
    {
        private MajdaService _majdaService;
        private HttpClient _httpClient;

        private Random _rand;
        public HomeController(MajdaService majdaService, HttpClient httpClient)
        {
            this._majdaService = majdaService;
            this._httpClient = httpClient;

            this._rand = new Random();
        }

        public IResult Google()
        {
            return new Redirect("http://www.google.com");
        }

        public IResult TestView()
        {
            var model = new ArticleViewModel();
            model.Text = $"Omegapog - your random number - {this._rand.Next(0, 100)}";

            return new MajdaView<ArticleViewModel>("App/Views/Home.majdahtml", model);
        }

        [FromMethod(Method.POST)]
        public IResult Test(string body)
        {
            Console.WriteLine($"Body from HomeController: {body}");

            return new Redirect("http://www.google.com");
        }

        [FromMethod(Method.POST)]
        [FromJson]
        public IResult JsonTest(ArticleViewModel article)
        {
            Console.WriteLine($"Json from HomeCotroller: {article.Text}");
            article.Text = "This totally works!";

            return new JsonResult(article);
        }

        [FromMethod(Method.POST)]
        public IResult JsonTest()
        {
            return new JsonResult(new ArticleViewModel() { Text = "New generated article!" });
        }

        [FromMethod(Method.POST)]
        [FromForm]
        public IResult FormTest(FormContent content)
        {
            Console.WriteLine($"Received form content of number of parameters: {content.Form.Count}");
            return new Redirect("http://www.google.com");
        }
    }
}
