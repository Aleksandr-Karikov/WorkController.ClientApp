using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkController.Client.Commands;
using WorkController.Client.Http.RequstModels;
using WorkController.Client.Models;
using WorkController.Client.ViewModels.BaseViewModels;
using WorkController.Client.Views;
using WorkController.Common.Http.Helper;
using WorkController.Common.Http.Helper.ApiHelper;

namespace WorkController.Client.ViewModels
{
    internal class LoginViewModel:BaseViewModel
    {
        #region ctors
        public LoginViewModel(IHttpClientFactory _factory)
        {
            factory = _factory;
            LoginCommand = new LamdaCommand(OnLoginCommandExecute, CanLoginCommandExecute);
            RegisterCommand = new LamdaCommand(OnRegisterCommandExecute, CanRegisterCommandExecute);
        }
        #endregion
        #region Fields
        IHttpClientFactory factory;
        private string email;
        private string password;
        private string viewPassword;
        #endregion
        #region Props
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string ViewPassword
        {
            get => viewPassword;
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        password = value;
                    }
                    else
                    {

                        if (value.Length > password.Length)
                        {
                            var temp = password;
                            password = value;
                            password = temp + password.Replace("*", "");

                        }
                        else
                        {
                            var temp = password.Remove(value.Length);
                            password = value;
                            password = temp + password.Replace("*", "");
                        }
                    }
                    viewPassword = String.Empty;
                    for (int i = 0; i < value.Length; i++)
                    {
                        viewPassword += "*";
                    }
                    OnPropertyChanged(nameof(ViewPassword));
                }
                catch (Exception)
                {
                    viewPassword = String.Empty;
                    password = String.Empty;
                }
            }
        }
        #endregion
        #region Commands
        public ICommand LoginCommand { get; }
        private bool CanLoginCommandExecute(object p)
        {
            return true;
        }
        private async void OnLoginCommandExecute(object p)
        {
            #region Валидность полей
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Одно из полей пустое");
                return;
            }
            if (password.Contains("*"))
            {
                MessageBox.Show("Пароль не может содержать '*'");
                return;
            }
            if (password.Contains(" "))
            {
                MessageBox.Show("Пароль не может содержать пробелов");
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль не может быть таким коротким");
                return;
            }
            if (!RequestHelper.IsValidEmail(email))
            {
                MessageBox.Show("Email не корректен");
                return;
            }
            #endregion
            HttpResponseMessage response= null;
            try
            {
                response = await RequestHelper.SendPostRequest(ApiHelperUri.LoginUri, factory, new LoginModel()
                {
                    Email = this.Email,
                    Password = password
                });

                var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                MessageBox.Show("Приветствуем " + user.LastName + " " + user.FirstName);
                var newWindow = new MainWindow(new User(factory)
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = user.Token,
                    ID = user.ID,
                    ChiefId = user.ChiefId
                });
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                window.Close();
                newWindow.Show();

            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Отсутствует подключение к серверу");
            }
            catch (JsonException)
            {

                var mes = await response.Content.ReadAsStringAsync();
                MessageBox.Show(mes);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public ICommand RegisterCommand { get; }
        private bool CanRegisterCommandExecute(object p) => true;
        private void OnRegisterCommandExecute(object p)
        {
            var a = new Register(factory);
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Close();
            a.Show();

        }
        #endregion
    }
}
