using Mobiles_accounting.Views;
using System;
using System.Windows;

namespace Mobiles_accounting
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartLabel.Visibility = Visibility.Visible;
            this.DataContext = new ViewModel();
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            StartLabel.Visibility = Visibility.Hidden;

            ResizeMode = ResizeMode.CanResize;


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow
            {
                Owner = this,
                DataContext = this.DataContext
            };
            (DataContext as ViewModel).OpenCommand.Execute("AddEq");
            addWindow.ShowDialog();
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceWindow replaceWindow = new ReplaceWindow
            {
                Owner = this,
                DataContext = this.DataContext
            };
            (DataContext as ViewModel).OpenCommand.Execute("ReplaceEq");
            replaceWindow.ShowDialog();
        }

        private void AllDataButton_Click(object sender, RoutedEventArgs e)
        {
            AllDataWindow allDataWindow = new AllDataWindow
            {
                Owner = this,
                DataContext = this.DataContext
            };
            allDataWindow.Show();
        }

        private void History_click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow
            {
                Owner = this,
                DataContext = this.DataContext
            };
            historyWindow.Show();
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            OtchetWindow otchetWindow = new OtchetWindow
            {
                Owner = this,
                DataContext = this.DataContext
            };
            otchetWindow.Show();
        }
    }
}
