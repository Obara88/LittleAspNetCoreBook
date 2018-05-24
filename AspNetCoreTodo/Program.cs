using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTodo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            //BuildWebHost2(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseKestrel(ConfiguracaoKestrel2).Build();

        public static IWebHost BuildWebHost2(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseKestrel(actionKestrel).Build();
        public static Action<KestrelServerOptions> actionKestrel = ConfiguracaoKestrel;
        public static void ConfiguracaoKestrel(KestrelServerOptions teste) { teste.ConfigureEndpoints(); }
        public static void ConfiguracaoKestrel2(KestrelServerOptions teste) => teste.ConfigureEndpoints();
    }    
}