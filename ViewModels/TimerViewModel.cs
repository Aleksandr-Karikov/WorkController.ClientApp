using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkController.Client.Commands;
using WorkController.Client.Models;
using WorkController.Client.ViewModels.BaseViewModels;

namespace WorkController.Client.ViewModels
{
    
    internal class TimerViewModel:BaseViewModel
    {
        private User user;

        public TimerViewModel(User user)
        {
            this.user = user;
            
            StartCommand = new LamdaCommand(OnStartCommandExecute, CanStartCommandExecute);
            StopCommand = new LamdaCommand(OnStopCommandExecute, CanStopCommandExecute);
            SendCommand = new LamdaCommand(OnSendCommandExecute, CanSendCommandExecute);
        }

        private string timer = "00:00:00";

        public string Timer 
        {
            get => timer;
            set
            {
                timer = value;
                OnPropertyChanged(nameof(Timer));
            }
        }
        private void UpdateTimer()
        {
            Timer = user.GetStringTime();
        }

        private async void SetTime()
        {
            while (user.IsTimerEnabled())
            {
                Timer = user.GetStringTime();
                if (user.GetTime().Hours>=24)
                {
                    await SendRezult();
                }
                Thread.Sleep(1000);
            }
            
        }
        public ICommand StartCommand { get; }
        private bool CanStartCommandExecute(object p)
        {
            if (user.IsTimerEnabled()) return false;
            return true;
        }
        private async void OnStartCommandExecute(object p)
        {
            user.StratTimer();
            await Task.Run(() => SetTime());
        }

        public ICommand StopCommand { get; }
        private bool CanStopCommandExecute(object p)
        {
            if (user.IsTimerEnabled()) return true;
            return false;
        }
        private void OnStopCommandExecute(object p)
        {
            user.StopTimer();
        }


        public ICommand SendCommand { get; }
        private bool CanSendCommandExecute(object p)
        {
            if (user.GetTime()==TimeSpan.MinValue) return false;
            return true;
        }
        public async Task SendRezult()
        {
            await user.SendTime();
            UpdateTimer();
        }
        private async void OnSendCommandExecute(object p)
        {
            await SendRezult();
        }
        

    }
}
