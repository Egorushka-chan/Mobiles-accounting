using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobiles_accounting.Model
{
    class Note : BaseVM
    {
        private int _EntranceNum;
        public int EntranceNum
        {
            get => _EntranceNum;
            set
            {
                _EntranceNum = value;
                OnPropertyChanged(nameof(EntranceNum));
            }
        }

        private int _ActionID;
        public int ActionID
        {
            get => _ActionID;
            set
            {
                _ActionID = value;
                OnPropertyChanged(nameof(EntranceNum));
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get => _Date;
            set
            {
                _Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string _Action;
        public string Action
        {
            get => _Action;
            set
            {
                _Action = value;
                OnPropertyChanged(nameof(Action));
            }
        }
    }
}
