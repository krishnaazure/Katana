﻿using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace KatanaIntro
{
    using System.IO;
    using System.Web.Http;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8000";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started");
                Console.ReadKey();
                Console.WriteLine("Stopped");
            }
        }
    }
    public class Startup
    {
        
        
        public void Configuration(IAppBuilder app)
        {
             app.Use(async (enviornment, next) =>
            {

                Console.WriteLine("Requesting :" + enviornment.Request.Path);

                await next();

                Console.WriteLine("Response :" + enviornment.Response.StatusCode);
 
            });

            ConfigureWebApi(app);
            app.UseHelloWorld();
       }

        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi",
                "api/{controller}/id", new { id = RouteParameter.Optional });
            app.UseWebApi(config);
           
        }
    }

    public static class AppBuilderExtensions
    {
        public static void UseHelloWorld(this IAppBuilder app)
        {
            app.Use<HelloWorldComponent>();
        }
    }

     public class HelloWorldComponent
     {
            AppFunc _next;
            public HelloWorldComponent(AppFunc next)
            {
                _next = next ;
            }

            public   Task Invoke(IDictionary<string,object> enviornment)
            {
             
            var response = enviornment["owin.ResponseBody"] as Stream;

                using (var writer = new StreamWriter(response))
                {
                    return writer.WriteAsync("Helddddlo!");
                }

            }
    }
}

 