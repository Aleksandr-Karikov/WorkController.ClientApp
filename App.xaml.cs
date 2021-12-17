using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WorkController.Client.Views;

namespace WorkControllerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private LoginVewModel LoginVM { get; set; }
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Login>();
            services.AddSingleton<Register>();
            services.AddHttpClient("WorkController", c =>
            {
                c.BaseAddress = new Uri("http://localhost:6341/");
            });
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var loginView = _serviceProvider.GetService<Login>();
            loginView.Show();
        }
    }
}
