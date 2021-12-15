using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkController.Client.Commands;
using WorkController.Client.Models;
using WorkController.Client.ViewModels.BaseViewModels;
using WorkController.Client.Views;

namespace WorkController.Client.ViewModels
{
    internal class MainWindowViewModel:BaseViewModel
    {
        private TimerViewModel TymerVM { get; set; }
        private HistoryViewModel HistoryVM { get; set; }
        public MainWindowViewModel(User user)
        {
            this.user = user;
            TymerVM = new TimerViewModel();
            curentView = TymerVM;
            HistoryVM = new HistoryViewModel();

            HistoryCommand = new LamdaCommand(OnHistoryCommandExecute, CanHistoryCommandExecute);
            TimerCommand = new LamdaCommand(OnTimerCommandExecute, CanTimerCommandExecute);
        }
        private User user;

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
        }
    }
}
