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
using WorkController.Client.ViewModels.BaseViewModels;
using WorkController.Client.Views;
using WorkController.Common.Http.Helper;
using WorkController.Common.Http.Helper.ApiHelper;

namespace WorkController.Client.ViewModels
{
    internal class RegisterViewModel:BaseViewModel
    {
        #region ctors
        public RegisterViewModel(IHttpClientFactory _factory)
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
        private string firstName;
        private string lastName;
        private int? chiefId;
        private bool isChecked;
        private Visibility pwdVisible;
        private Visibility lFVisible;
        #endregion
        #region Props
        public Visibility PwdVisible
        {
            get => pwdVisible;
            set
            {
                pwdVisible = value;
                OnPropertyChanged(nameof(PwdVisible));
            }
        }
        public Visibility LFVisible
        {
            get => lFVisible;
            set
            {
                lFVisible = value;
                OnPropertyChanged(nameof(LFVisible));
            }
        }
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked)
                    value = false;
                if (value)
                {
                    LFVisible = Visibility.Hidden;
                    PwdVisible = Visibility.Hidden;
                }
                else
                {
                    LFVisible = Visibility.Visible;
                    PwdVisible = Visibility.Visible;
                }
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
        public int? ChiefId
        {
            get => chiefId;
            set
            {
                chiefId = value;
                OnPropertyChanged(nameof(ChiefId));
            }
        }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        #endregion
        #region Commands
        public ICommand RegisterCommand { get; }
        private bool CanRegisterCommandExecute(object p)
        {
            return true;
        }
        private async void OnRegisterCommandExecute(object p)
        {
            #region Валидность полей
            if (IsChecked)
            {
                if (string.IsNullOrEmpty(email) || ChiefId == null)
                {
                    MessageBox.Show("Одно из полей пустое");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || ChiefId == null)
                {
                    MessageBox.Show("Одно из полей пустое");
                    return;
                }
                if (password.Length < 6)
                {
                    MessageBox.Show("Пароль слишком короткий");
                    return;
                }
            }        
            if (!RequestHelper.IsValidEmail(Email))
            {
                MessageBox.Show("Email не корректен");
                return;
            }
            #endregion
            try
            {
                var response = await RequestHelper.SendPostRequest(ApiHelperUri.RegisterUri, factory, new RegisterModel()
                {
                    Email = email,
                    Password = password,
                    LastName = lastName,
                    FirstName = firstName,
                    ChiefID = chiefId
                });
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Отсутствует подключение к серверу");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public ICommand LoginCommand { get; }
        private bool CanLoginCommandExecute(object p) => true;
        private void OnLoginCommandExecute(object p)
        {
            var a = new Login(factory);
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Close();
            a.Show();

        }
        #endregion
    }
}
