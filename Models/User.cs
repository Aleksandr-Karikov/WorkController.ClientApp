using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkController.Common;
using Newtonsoft.Json;
using System.Diagnostics;
using WorkController.Common.Http.Helper;

using System.IO.IsolatedStorage;
using System.IO;
using WorkController.Common.Http.Helper.ApiHelper;
using WorkController.Client.Http.RequstModels;

namespace WorkController.Client.Models
{
    public  class User : INotifyPropertyChanged
    {
        public User(IHttpClientFactory _factory)
        {
            factory = _factory;
            stopwatch = new Stopwatch();
        }
        public User(){}
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #region Fields
        private IHttpClientFactory factory;
        private string email;
        private string firstName;
        private string lastName;
        private string token;
        private int id;
        private int chiefId;
        private Stopwatch stopwatch;
        #endregion
        #region Properties
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
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
        public string Token
        {
            get => token;
            set
            {
                token = value;
                OnPropertyChanged(nameof(Token));
            }
        }
        public int ChiefId
        {
            get => chiefId;
            set
            {
                chiefId = value;
                OnPropertyChanged(nameof(ChiefId));
            }
        }
        #endregion

        public async Task<bool> IsConnectionAlive()
        {
            return await RequestHelper.IsConnectionAlive(factory);
        }
        //private void SaveDatasLocal()
        //{
        //    States.StartTime = stopwatch.Elapsed.ToString();
        //    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
        //    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("Timer", FileMode.Create, storage))
        //    using (StreamWriter writer = new StreamWriter(stream))
        //    {
        //        writer.WriteLine(States.StartTime);
        //    }
        //}
        public void StratTimer()
        {
            stopwatch.Start();
        }
        public void StopTimer()
        {
            stopwatch.Stop();
        }
        public async Task SendTime()
        {

            await RequestHelper.SendPostAuthRequest(ApiHelperUri.SetTime, factory, Token, new TimeModel()
            {
                Date = DateTime.Now.Date,
                Id = ID,
                Time = (int)stopwatch.ElapsedMilliseconds
            }) ;
            //SaveDatasLocal();
            stopwatch.Reset();
        }
        public bool IsTimerEnabled()
        {
            return stopwatch.IsRunning;
        }
        public string GetStringTime()
        {
            TimeSpan time;
            time = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
            time.Hours, time.Minutes, time.Seconds);

            return elapsedTime;
        }
        public TimeSpan GetTime()
        {

            return stopwatch.Elapsed;
        }
    }
}
