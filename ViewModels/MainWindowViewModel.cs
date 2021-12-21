using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkController.Client.Commands;

using WorkController.Client.Models;
using WorkController.Client.ViewModels.BaseViewModels;
using WorkController.Client.Views;
using WorkController.Common.Http.Helper;

namespace WorkController.Client.ViewModels
{
    internal class MainWindowViewModel:BaseViewModel
    {
        private TimerViewModel TymerVM { get; set; }
        private HistoryViewModel HistoryVM { get; set; }
        public MainWindowViewModel(User user)
        {
            this.user = user;
            TymerVM = new TimerViewModel(user);
            curentView = TymerVM;
            HistoryVM = new HistoryViewModel(user);

            HistoryCommand = new LamdaCommand(OnHistoryCommandExecute, CanHistoryCommandExecute);
            TimerCommand = new LamdaCommand(OnTimerCommandExecute, CanTimerCommandExecute);
            CloseCommand = new LamdaCommand(OnCloseCommandExecute, CanCloseCommandExecute);
            ChangeUserCommand = new LamdaCommand(OnChangeUserCommandExecute, CanChangeUserCommandExecute);
            OnStart();



        }
        private readonly User user;

        private object curentView;
        public object CurentView
        {
            get => curentView;
            set
            {
                curentView = value;
                OnPropertyChanged(nameof(CurentView));
            }
        }

        private async void OnStart()
        {
            await Task.Run(() => CheckConnection());
        }

        public ICommand TimerCommand { get; }
        private bool CanTimerCommandExecute(object p)
        {
            return true;
        }
        private void OnTimerCommandExecute(object p)
        {
            CurentView = TymerVM;
        }

        public ICommand HistoryCommand { get; }
        private bool CanHistoryCommandExecute(object p)
        {
            return true;
        }
        private void OnHistoryCommandExecute(object p)
        {
            CurentView = HistoryVM;
            HistoryVM.Update();
        }

        public ICommand CloseCommand { get; }
        private bool CanCloseCommandExecute(object p)
        {
            return true;
        }

        private async void OnCloseCommandExecute(object p)
        {
            await TymerVM.SendRezult();
            
            Application.Current.Shutdown();
        }


        private async void CheckConnection()
        {
            var flag = true;
            while (true)
            {
                flag = await user.IsConnectionAlive();
                if (!flag)
                {
                    MessageBox.Show("Подключение разорвано");
                    Thread.Sleep(10000);
                }
            }
        }

        public ICommand ChangeUserCommand { get; }
        private bool CanChangeUserCommandExecute(object p)
        {
            return true;
        }
        private void OnChangeUserCommandExecute(object p)
        {

            new Login(user.Factory).Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Login) continue;
                window.Close();
            }
        }
    }
}
