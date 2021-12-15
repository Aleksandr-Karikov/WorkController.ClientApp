using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using WorkController.Client.ViewModels;

namespace WorkController.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login(IHttpClientFactory factory)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(factory);
        }
    }
}
