using Mobiles_accounting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobiles_accounting.Model
{
    class Program : BaseVM
    {
        private int id;
        private string name;
        private Developer dev;

        private int quantity;

        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Developer Developer
        {
            get => dev;
            set
            {
                dev = value;
                OnPropertyChanged(nameof(Developer));
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

    }
}
