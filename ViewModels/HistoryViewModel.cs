using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Client.Models;

namespace WorkController.Client.ViewModels
{
    internal class HistoryViewModel
    {
        private readonly User user;
        public HistoryViewModel(User user)
        {
            this.user = user;
        }


    }
}
