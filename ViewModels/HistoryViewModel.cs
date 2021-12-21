using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Admin.Models;
using WorkController.Client.Models;
using WorkController.Client.ViewModels.BaseViewModels;

namespace WorkController.Client.ViewModels
{
    internal class HistoryViewModel:BaseViewModel
    {
        private bool radioButtonFilter;
        private readonly User user;
        private DateTime selectedDate = DateTime.Now;
        public HistoryViewModel(User user)
        {
            this.user = user;
            Update();
        }
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public ObservableCollection<Time> Times { get; set; } =
            new ObservableCollection<Time> { };
        public ObservableCollection<Time> CurentTimes { get; set; } =
            new ObservableCollection<Time> { };
        public async void Update()
        {
            Times.Clear();
            var times = await user.GetTimes(user.Factory, user.Token);
            if (times != null)
                foreach (var time in times)
                {
                    Times.Add(time);
                }

            UpdateCurent();
        }
        private void UpdateCurent()
        {
            CurentTimes.Clear();
            if (!RadioButtonFilter)
            {
                foreach (var time in Times)
                {
                    CurentTimes.Add(time);
                }
                return;
            }
            foreach (var time in Times)
            {
                if (SelectedDate.Date == time.DateTime.Date)
                {
                    CurentTimes.Add(time);
                }
            }
        }

        public bool RadioButtonFilter
        {
            get => radioButtonFilter;
            set
            {
                if (radioButtonFilter)
                    radioButtonFilter = false;
                else radioButtonFilter = true;
                OnPropertyChanged(nameof(RadioButtonFilter));
                UpdateCurent();
            }
        }

    }
}
