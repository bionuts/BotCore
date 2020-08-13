using BCore.Contracts;
using BCore.Data;
using BCore.Forms;
using BCore.Lib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);
            
            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            Form mainBotForm = serviceProvider.GetRequiredService<MainBotForm>();
            Application.Run(mainBotForm);

            // Application.Run(new MainBotForm());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IBotDatabaseRepository, BotDatabaseRepository>();
            services.AddScoped<IMobinBroker, MobinBroker>();
            services.AddScoped<IMofidBroker, MofidBroker>();
            services.AddScoped<MainBotForm>();
            services.AddScoped<Account>();
        }
    }
}
