using System.Windows;

namespace Mobiles_accounting
{
    /// <summary>
    /// Логика взаимодействия для AddStateWindow.xaml
    /// </summary>
    public partial class AddStateWindow : Window
    {
        public AddStateWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
