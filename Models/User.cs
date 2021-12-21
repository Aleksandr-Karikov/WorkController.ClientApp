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
using WorkController.Admin.Models;
using System.Drawing;

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
        private int screenShotPeriod;
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
        public IHttpClientFactory Factory
        {
            get => factory;
            set
            {
                factory = value;
                OnPropertyChanged(nameof(Factory));
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
        public int ScreenShotPeriod
        {
            get => screenShotPeriod;
            set
            {
                screenShotPeriod = value;
                OnPropertyChanged(nameof(ScreenShotPeriod));
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
        private byte[] ConvertImageToBinary(Image image)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        public List<Image> GetImages()
        {
            List<Image> images = new List<Image>();
            string fullpath = Info.PathStorage + "Screenshots" +"\\";
            try
            {
                for (int i = 0; ; i++)
                {
                    Image img;
                    using (var bmpTemp = new Bitmap(fullpath + ID.ToString() + "_" + i.ToString() + ".jpg"))
                    {
                        img = new Bitmap(bmpTemp);
                    }
                    //var ima = Image.FromFile(fullpath + ID.ToString() + "_" + i.ToString() + ".jpg");
                    images.Add(img);
                }
            }
            catch { return images; }
            
        }
        public async Task SendTime()
        {
            var images = GetImages();
            List<byte[]> screens = null;
            if (images!=null)
            {
                screens = new();
                foreach (var image in images)
                {
                    screens.Add(ConvertImageToBinary(image));
                }
            }
            await RequestHelper.SendPostAuthRequest(ApiHelperUri.SetTime, factory, Token, new TimeModel()
            {
                Date = DateTime.Now.Date,
                Id = ID,
                Time = (int)stopwatch.ElapsedMilliseconds,
                Screens = screens

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

        internal async Task<IEnumerable<Time>> GetTimes(IHttpClientFactory factory, string Token)
        {

            var response = await RequestHelper.SendPostAuthRequest(ApiHelperUri.GetTimesUri + $"?ID={ID}", factory, Token);
            var content = await response.Content.ReadAsStringAsync();
            var rezult = JsonConvert.DeserializeObject<List<Time>>(content);
            foreach (var record in rezult)
            {
                var time = TimeSpan.FromMilliseconds(record.Milleseconds);
                record.TimeString = String.Format("{0:00}:{1:00}:{2:00}",
            time.Hours, time.Minutes, time.Seconds);
            }
            return rezult;
        }

    }
}
