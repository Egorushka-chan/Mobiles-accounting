using Mobiles_accounting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobiles_accounting.Model
{
    class Check : BaseVM
    {
        private int id;
        private string username;
        private DateTime date;
        private Program prog;

        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public string UserName
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public Program Program
        {
            get => prog;
            set
            {
                prog = value;
                OnPropertyChanged(nameof(Program));
            }
        }
    }
}
