using System.Windows;

namespace Mobiles_accounting
{
    /// <summary>
    /// Логика взаимодействия для AllDataWindow.xaml
    /// </summary>
    public partial class AllDataWindow : Window
    {
        public AllDataWindow()
        {
            InitializeComponent();
        }

        private void AddStateButton_Click(object sender, RoutedEventArgs e)
        {
            AddStateWindow addStateWindow = new AddStateWindow
            {
                DataContext = this.DataContext
            };
            (DataContext as ViewModel).OpenCommand.Execute("AddSt");
            addStateWindow.ShowDialog();
        }

        private void AddSubdivisionButton_Click(object sender, RoutedEventArgs e)
        {
            AddSubdivisionWindow addSubdivisionWindow = new AddSubdivisionWindow
            {
                DataContext = this.DataContext
            };
            (DataContext as ViewModel).OpenCommand.Execute("AddSub");
            addSubdivisionWindow.ShowDialog();
        }
    }
}
