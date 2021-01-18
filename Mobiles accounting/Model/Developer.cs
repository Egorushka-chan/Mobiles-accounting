using Mobiles_accounting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobiles_accounting.Model
{
    class Developer : BaseVM
    {
        private int id;
        private string name;

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
    }
}
