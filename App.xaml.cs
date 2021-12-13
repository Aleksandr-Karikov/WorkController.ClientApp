using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WorkControllerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<LoginView>();
            //services.AddSingleton<RegisterView>();
            //services.AddHttpClient("WorkController", c =>
            //{
            //    c.BaseAddress = new Uri("http://localhost:6341/");
            //    // Github API versioning
            //    c.DefaultRequestHeaders.Add("Accept", "application/json");
            //    // Github requires a user-agent
            //    //c.DefaultRequestHeaders.Add("Token", Token.AcceptToken);
            //});
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            //var loginView = _serviceProvider.GetService<LoginView>();
            //loginView.Show();
        }
    }
}
