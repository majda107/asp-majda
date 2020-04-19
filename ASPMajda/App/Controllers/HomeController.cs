﻿using ASPMajda.App.Data;
using ASPMajda.App.Models;
using ASPMajda.Models;
using ASPMajda.Models.Attributes;
using ASPMajda.Models.Result;
using ASPMajda.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPMajda.App.Controllers
{
    class HomeController: ControllerBase
    {
        private MajdaService _majdaService;
        public HomeController(MajdaService majdaService)
        {
            this._majdaService = majdaService;
        }

        public IResult Google()
        {
            return new Redirect("http://www.google.com");
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

            return new Redirect("https://www.google.com");
        }
    }
}