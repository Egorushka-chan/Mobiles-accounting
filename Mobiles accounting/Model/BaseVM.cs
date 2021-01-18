using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mobiles_accounting.Model
{
    abstract class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string properity = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(properity));
        }
    }
}
