using System.Windows;

namespace Mobiles_accounting
{
    /// <summary>
    /// Логика взаимодействия для AddSubdivisionWindow.xaml
    /// </summary>
    public partial class AddSubdivisionWindow : Window
    {
        public AddSubdivisionWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
