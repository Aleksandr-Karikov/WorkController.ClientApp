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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace WorkController.Client.ViewModels
{
    
    internal class TimerViewModel:BaseViewModel
    {
        private User user;

        public TimerViewModel(User user)
        {
            this.user = user;
            DeleteFolder();
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
        private void CreateFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    return;
                }
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
            }
        }

        private void DeleteFolder()
        {
            try
            {
                string fullpath = Info.PathStorage + "\\" + "Screenshots";


                System.IO.DirectoryInfo di = new DirectoryInfo(fullpath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

            }
            catch (Exception)
            { 
            }
        }

        private void FullScreenshot(String filename, ImageFormat format)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                string fullpath = Info.PathStorage + "\\" + "Screenshots";
                CreateFolder(fullpath);
                bitmap.Save(fullpath + "\\" + filename, format);
            }
        }
        private int lastMinute;
        private int counter=0;
        private async void SetTime()
        {
            while (user.IsTimerEnabled())
            {
                if (user.ScreenShotPeriod!=0)
                    if (user.GetTime().Minutes % user.ScreenShotPeriod == 0 && user.GetTime().Minutes!=0 && lastMinute!= user.GetTime().Minutes)
                    {
                        FullScreenshot(user.ID + "_" + counter.ToString() + ".jpg", ImageFormat.Jpeg);
                        lastMinute = user.GetTime().Minutes;
                        counter++;
                    }
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
            DeleteFolder();
            counter = 0;
            UpdateTimer();
        }
        private async void OnSendCommandExecute(object p)
        {
            await SendRezult();
        }
        

    }
}
